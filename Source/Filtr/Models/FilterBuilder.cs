using Filtr.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Filtr.Models
{
    /// <summary> Property mappers </summary>
    /// <typeparam name="TFromEntity">Object to map from</typeparam>
    /// <typeparam name="TToEntity">Object to map to</typeparam>
    public class FilterBuilder<TFromEntity, TToEntity> where TFromEntity : class where TToEntity : class
    {
        private static Dictionary<string, FilterSetting> _filterSetting;

        /// <summary> Property mappers </summary>
        /// <typeparam name="TFromEntity">Object to map from</typeparam>
        /// <typeparam name="TToEntity">Object to map to</typeparam>
        public FilterBuilder(Dictionary<string, FilterSetting> filterSetting)
        {
            _filterSetting = filterSetting;
        }

        /// <summary>
        /// Maps property of filter object to certain object typed stored in database
        /// </summary>
        /// <typeparam name="TFromProperty">Property type to bind from</typeparam>
        /// <typeparam name="TToProperty">Property type to bind to</typeparam>
        /// <param name="fromPredicate">Property to bind from</param>
        /// <param name="toPredicate">Property to bind to</param>
        /// <param name="operatorBetween">Operator between conditions in db query</param>
        /// <param name="method">Method to call for property</param>
        /// <param name="sorting">Sorting type</param>
        public FilterBuilder<TFromEntity, TToEntity> Map<TFromProperty, TToProperty>(
            Expression<Func<TFromEntity, TFromProperty>> fromPredicate,
            Expression<Func<TToEntity, TToProperty>> toPredicate,
            OperatorBetween operatorBetween,
            Method method,
            SortingDirection sortingDirection = SortingDirection.Asc)
        {
            // get full property names
            var fromPropertyName = GetFullPropertyName(fromPredicate);

            var toMember = toPredicate.Body as MemberExpression;

            var propertyPath = GetPropertyPath(toPredicate.Body as MemberExpression);

            // create filter setting which stores all data about filter
            var filterSetting = new FilterSetting
            {
                Name = fromPropertyName,
                PropertyPath = propertyPath,
                OperandType = toMember.Member.DeclaringType,
                ParameterType = toMember.Type,
                OperatorBetween = operatorBetween,
                MethodName = method,
                SortingDirection = sortingDirection
            };

            _filterSetting.Add(fromPropertyName, filterSetting);

            return this;
        }

        /// <summary>
        /// Configure default sorting value for object type
        /// </summary>
        /// <typeparam name="TToProperty">Type default value configured for</typeparam>
        /// <param name="toPredicate">Property for default sorting</param>
        /// <param name="sortingDirection">Sorting direction</param>
        public FilterBuilder<TFromEntity, TToEntity> Default<TToProperty>(
           Expression<Func<TToEntity, TToProperty>> toPredicate,
           SortingDirection sortingDirection = SortingDirection.Asc)
        {
            //var defaultName = $"{typeof(TToEntity).FullName}.Default";
            var defaultName = $"{typeof(TFromEntity).FullName}.Default";

            var toMember = toPredicate.Body as MemberExpression;

            var propertyPath = GetPropertyPath(toPredicate.Body as MemberExpression);

            // create filter setting which stores all data about filter
            var filterSetting = new FilterSetting
            {
                Name = defaultName,
                PropertyPath = propertyPath,
                OperandType = toMember.Member.DeclaringType,
                ParameterType = toMember.Type,
                OperatorBetween = OperatorBetween.None,
                MethodName = Method.Equal,
                SortingDirection = sortingDirection
            };

            _filterSetting.Add(defaultName, filterSetting);

            return this;
        }

        /// <summary>
        /// Return full property name of predicate's body
        /// </summary>
        private string GetFullPropertyName<TProperty>(Expression<Func<TFromEntity, TProperty>> predicate)
        {
            var fromPropertyNames = string.Join(".", predicate.Body.ToString().Split('.').Skip(1));

            return $"{predicate.Parameters[0].Type.FullName}.{fromPropertyNames}";
        }

        /// <summary>
        /// Return property path as array
        /// </summary>
        private string[] GetPropertyPath(MemberExpression member)
        {
            return member.ToString().Split('.').Skip(1).ToArray();
        }
    }
}