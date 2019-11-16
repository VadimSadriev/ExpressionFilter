using System.Collections.Generic;

namespace Filtr.Models
{
    /// <summary> Poco containing filter/sorting data </summary>
    public class FilterData
    {
        /// <summary> List of filters to apply to query </summary>
        public Filter[] Filters { get; set; } = new Filter[0];

        /// <summary> List of sortings to apply to query </summary>
        public Sorting[] Sortings { get; set; } = new Sorting[0];
    }
}