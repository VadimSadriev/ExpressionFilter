using Filtr.Exceptions;
using Filtr.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Filtr.Extensions
{
    /// <summary> Extensions for <see cref="Filtrator"/> </summary>
    public static class FiltratorExtensions
    {
        /// <summary>
        /// Apples filters to the query based on requested data
        /// </summary>
        /// <typeparam name="T">Type of filtred objects</typeparam>
        /// <typeparam name="TFilterDto">Type of dto get data from</typeparam>
        /// <param name="query">Query to apply filters to</param>
        /// <param name="filterRequest">Filter request containing data about filtering</param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T, TFilterDto>(this IQueryable<T> query, PagedFilterRequest<TFilterDto> filterRequest)
            where TFilterDto : class
        {
            if (filterRequest == null || filterRequest.FilterDto == null)
                throw new FilterRequestException("Data for filtering is invalid");

            // prepare data for filtering
            var filterData = new FilterData
            {
                Filters = GetFiltersFromDto(filterRequest.FilterDto),
                Sortings = GetSortingsFromRequest<TFilterDto>(filterRequest.Sortings)
            };

            return Filtrator.Filter<T>(query, filterData);
        }

        /// <summary>
        /// Return properties and values from dto
        /// </summary>
        private static List<Filter> GetFiltersFromDto<TDto>(TDto dto)
        {
            var filterDtoTypeFullName = dto.GetType().FullName;

            return dto.GetType()
                  .GetProperties()
                  .Select(x => new
                  {
                      Name = x.Name,
                      Value = x.GetValue(dto)
                  })
                  .Where(x => x.Value != null)
                  .Select(x =>
                  {
                      var values = new List<string>();

                      // if collection of values convert it to list of strings
                      // otherwise just create new list with one value
                      if (x.Value is ICollection)
                          values = ((ICollection)x.Value).Cast<object>().Select(o => o.ToString()).ToList();
                      else
                          values = new List<string> { x.Value.ToString() };

                      return new Filter
                      {
                          Name = $"{filterDtoTypeFullName}.{x.Name}",
                          Values = values
                      };
                  }).ToList();
        }

        /// <summary>
        /// Prepares sorting list for internal filters
        /// </summary>
        private static List<Sorting> GetSortingsFromRequest<TDto>(List<Sorting> sortings)
        {
            var filterDtoTypeFullName = typeof(TDto).FullName;

            if (sortings.Count == 0)
                return new List<Sorting>
                {
                    new Sorting
                    {
                        Name = $"{filterDtoTypeFullName}.Default",
                    }
                };


            return sortings
                .Select(x => new Sorting
                {
                    Name = $"{filterDtoTypeFullName}.{x.Name}",
                    Direction = x.Direction,
                    Priority = x.Priority
                }).ToList();
        }
    }
}
