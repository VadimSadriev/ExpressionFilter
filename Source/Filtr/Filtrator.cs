using Filtr.Enums;
using Filtr.Exceptions;
using Filtr.Interfaces;
using Filtr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Filtr
{
    /// <summary> Base class for all filters </summary>
    public static class Filtrator
    {
        #region Configuration

        /// <summary>
        /// Setting for all registered filters
        /// </summary>
        private static Dictionary<string, FilterSetting> _filterSettings = new Dictionary<string, FilterSetting>();

        /// <summary>
        /// Gets all types from <see cref="Assembly"/> which implemented <see cref="IFilterConfiguration{TFromEntity, TToEntity}"/>
        /// and invokes their <see cref="IFilterConfiguration{TFromEntity, TToEntity}.Configure(FilterBuilder{TFromEntity, TToEntity})"/>
        /// method to setup filter settings
        /// </summary>
        /// <param name="assembly">Assembly to get types from</param>
        public static void ConfigureFilters(Assembly assembly)
        {
            // filter configuration type
            var filterBaseType = typeof(IFilterConfiguration<,>);

            // get all types which implemented IFilterConfiguration<,>
            var filterTypes = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i => i.Name == filterBaseType.Name))
                .ToList();

            foreach (var filterType in filterTypes)
            {
                // get types IFilterConfiguration<,>
                var filertConfigurationInterface = filterType.GetInterface(filterBaseType.Name);

                // generic types for filter builder
                var genericTypes = filertConfigurationInterface.GetGenericArguments();

                // get typed filter builder type
                var filterBuilderType = typeof(FilterBuilder<,>).MakeGenericType(genericTypes);

                // new instance of filter builder
                var filterBuilder = Activator.CreateInstance(filterBuilderType, new object[] { _filterSettings });

                // filter instance
                var filter = Activator.CreateInstance(filterType);

                // get method info for Configure
                var configureMethodInfo = filterType.GetMethod("Configure");

                // Invoke Configure to setup filter settings
                configureMethodInfo.Invoke(filter, new object[] { filterBuilder });
            }
        }

        /// <summary>
        /// Clear setting dictionary
        /// </summary>
        public static void ClearSettings()
        {
            _filterSettings = new Dictionary<string, FilterSetting>();
        }

        #endregion

        #region Filters

        /// <summary>
        /// Applies filters to the query based on filterData
        /// </summary>
        /// <typeparam name="T">Type of filtering object</typeparam>
        /// <param name="query">Query to apply filters to</param>
        /// <param name="filterData">Data for filtering and sorting</param>
        public static IQueryable<T> Filter<T>(IQueryable<T> query, FilterData filterData)
        {
            // apply filters
            foreach (var filter in filterData.Filters)
            {
                // ensure we configured setting for property
                if (!_filterSettings.ContainsKey(filter.Name))
                    throw new FilterSettingNotFoundException(filter.Name);

                var propertyFilterSetting = _filterSettings[filter.Name];

                query = ApplyFilter(query, filter, propertyFilterSetting);
            }

            // apply sortings
            var sortings = filterData.Sortings.OrderBy(x => x.Priority).ToArray();

            var firstSorting = sortings[0];

            // ensure we configured setting for property
            if (!_filterSettings.ContainsKey(firstSorting.Name))
                throw new FilterSettingNotFoundException(firstSorting.Name);

            var propertySetting = _filterSettings[sortings[0].Name];

            var sortedQuery = ApplySorting(query, firstSorting, propertySetting);

            for (var i = 1; i < filterData.Sortings.Length; i++)
            {
                var sorting = sortings[i];

                // ensure we configured setting for property
                if (!_filterSettings.ContainsKey(sorting.Name))
                    throw new FilterSettingNotFoundException(sorting.Name);

                propertySetting = _filterSettings[sorting.Name];

                sortedQuery = ApplyNextSorting(sortedQuery, sorting, propertySetting);
            }

            return sortedQuery;
        }

        /// <summary>
        /// Applies passed filter to the query
        /// </summary>
        /// <typeparam name="T">Type of filtering objects</typeparam>
        /// <param name="query">Query apply filter to</param>
        /// <param name="filter">Poco contains filter values for filtering</param>
        /// <param name="filterSetting">Filter settings for filtering by passed property</param>
        private static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter, FilterSetting filterSetting)
        {
            // parameter for lambda expression
            var lambdaParameter = Expression.Parameter(typeof(T), "p");

            // building path to the property (x => x.Property.AnotherProperty)
            var propertyAccess = GetPropertyAccess(lambdaParameter, filterSetting.PropertyPath);

            // get constant expression for first value
            var constantExpression = GetConstantExpression(filterSetting.ParameterType, filter.Values[0]);

            // get first condition for lambda expression
            var condition = GetMethodExpression(filterSetting.MethodName, propertyAccess, constantExpression);

            // if we have more than one value for property
            // then make condition for all
            if (!filterSetting.IsSingleFilter)
            {
                for (var i = 1; i < filter.Values.Length; i++)
                {
                    var nextConstantExpression = GetConstantExpression(filterSetting.ParameterType, filter.Values[i]);
                    condition = GetNextMethodExpression(condition, filterSetting, propertyAccess, nextConstantExpression);
                }
            }

            var lambda = Expression.Lambda<Func<T, bool>>(condition, lambdaParameter);

            return query.Where(lambda);
        }

        /// <summary>
        /// Applies sorting to the query
        /// </summary>
        /// <typeparam name="T">Type of sorted object</typeparam>
        /// <param name="query">Query to apply sorting to</param>
        /// <param name="filterSetting">Filter settings for sorting by passed property</param>
        private static IOrderedQueryable<T> ApplySorting<T>(IQueryable<T> query, Sorting sorting, FilterSetting filterSetting)
        {
            // parameter for lambda expression
            var lambdaParameter = Expression.Parameter(typeof(T), "p");

            // building path to the property (x => x.Property.AnotherProperty)
            var propertyAccess = GetPropertyAccess(lambdaParameter, filterSetting.PropertyPath);

            if (sorting.Direction == SortingDirection.Asc)
                return CallSortingMethod(query, "OrderBy", filterSetting.ParameterType, propertyAccess, lambdaParameter);

            if (sorting.Direction == SortingDirection.Desc)
                return CallSortingMethod(query, "OrderByDescending", filterSetting.ParameterType, propertyAccess, lambdaParameter);

            return (IOrderedQueryable<T>)query;
        }

        /// <summary>
        /// Applies sorting to already sorted query
        /// </summary>
        /// <typeparam name="T">Type of sorted object</typeparam>
        /// <param name="query">Query to apply sorting to</param>
        /// <param name="filterSetting">Filter settings for sorting by passed property</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> ApplyNextSorting<T>(IOrderedQueryable<T> query, Sorting sorting, FilterSetting filterSetting)
        {
            // parameter for lambda expression
            var lambdaParameter = Expression.Parameter(typeof(T), "p");

            // building path to the property (x => x.Property.AnotherProperty)
            var propertyAccess = GetPropertyAccess(lambdaParameter, filterSetting.PropertyPath);

            if (sorting.Direction == SortingDirection.Asc)
                return CallSortingMethod(query, "ThenBy", filterSetting.ParameterType, propertyAccess, lambdaParameter);

            if (sorting.Direction == SortingDirection.Desc)
                return CallSortingMethod(query, "ThenByDescending", filterSetting.ParameterType, propertyAccess, lambdaParameter);

            return query;
        }


        /// <summary>
        /// Returns path to the property as expression
        /// </summary>
        private static Expression GetPropertyAccess(ParameterExpression lambdaParameter, string[] propertyPath)
        {
            return Normalize(propertyPath.Aggregate((Expression)lambdaParameter, Expression.Property));
        }

        /// <summary>
        /// Returns constant expression with value passed to the method
        /// </summary>
        /// <param name="parameterType">Type of value</param>
        /// <param name="value">Actual value</param>
        private static Expression GetConstantExpression(Type parameterType, string value)
        {
            // if value is enum parse as enum and return expression
            if (parameterType.IsEnum)
                return Expression.Constant(Enum.Parse(parameterType, value));

            // First check if our parameter is a nullable type
            var underNullableType = Nullable.GetUnderlyingType(parameterType);

            // if not nullable just return expression
            if (underNullableType == null)
                return Normalize(Expression.Constant(Convert.ChangeType(value, parameterType)));

            // if nullable convert to common type and return expression
            var constantExpression = Expression.Constant(Convert.ChangeType(value, underNullableType));
            return Normalize(Expression.Convert(constantExpression, parameterType));
        }

        /// <summary>
        /// Returns expression of called method with given value and property expression
        /// </summary>
        /// <param name="method">Method to call for property</param>
        /// <param name="propertyAccess">Access to property</param>
        /// <param name="constantExpression">Value passed to method</param>
        /// <returns></returns>
        private static Expression GetMethodExpression(Method method, Expression propertyAccess, Expression constantExpression)
        {
            switch (method)
            {
                case Method.Equal:
                    {
                        return Expression.Equal(propertyAccess, constantExpression);
                    }
                case Method.GreatherOrEqualThen:
                    {
                        return Expression.GreaterThanOrEqual(propertyAccess, constantExpression);
                    }
                case Method.GreatherThen:
                    {
                        return Expression.GreaterThan(propertyAccess, constantExpression);
                    }
                case Method.LessOrEqualThen:
                    {
                        return Expression.LessThanOrEqual(propertyAccess, constantExpression);
                    }
                case Method.LessThen:
                    {
                        return Expression.LessThan(propertyAccess, constantExpression);
                    }
                case Method.Contains:
                    {
                        return Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constantExpression);
                    }
                case Method.StartsWith:
                    {
                        return Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constantExpression);
                    }
                case Method.EndsWith:
                    {

                        return Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constantExpression);
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Applies addition condition to passed condition
        /// </summary>
        private static Expression GetNextMethodExpression(Expression condition, FilterSetting setting,
            Expression propertyAccess, Expression constantExpression)
        {
            switch (setting.OperatorBetween)
            {
                case OperatorBetween.Or:
                    {
                        return Expression.Or(condition, GetMethodExpression(setting.MethodName, propertyAccess, constantExpression));
                    }
                case OperatorBetween.And:
                    {
                        return Expression.And(condition, GetMethodExpression(setting.MethodName, propertyAccess, constantExpression));
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Normalizes string for filtering
        /// </summary>
        private static Expression Normalize(Expression expression)
        {
            if (expression.Type == typeof(string))
                return Expression.Call(expression, "ToUpper", null);

            return expression;
        }

        /// <summary>
        /// Calls sorting method with given name for passed query
        /// </summary>
        /// <typeparam name="T">Type of sorted object</typeparam>
        /// <param name="query">Query to call method for</param>
        /// <param name="sortingMethod">Method name to call</param>
        /// <param name="parameterType">Type of lambda parameter</param>
        /// <param name="propertyAccess">Path to the property as expression</param>
        /// <param name="lambdaParameter">Lambda parameter</param>
        /// <returns></returns>
        private static IOrderedQueryable<T> CallSortingMethod<T>(IQueryable<T> query, string sortingMethod,
            Type parameterType, Expression propertyAccess, ParameterExpression lambdaParameter)
        {
            // create method call expression for passed sorting method
            var callExpression = Expression.Call(
                typeof(Queryable),
                sortingMethod,
                new Type[] { typeof(T), parameterType },
                query.Expression,
                Expression.Lambda(propertyAccess, lambdaParameter));

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(callExpression);
        }

        #endregion
    }
}
