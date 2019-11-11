using System.Collections.Generic;

namespace Filtr.Models
{
    /// <summary> Poco containing filter/sorting data </summary>
    public class FilterData
    {
        /// <summary> List of filters to apply to query </summary>
        public List<Filter> Filters { get; set; } = new List<Filter>();

        /// <summary> List of sortings to apply to query </summary>
        public List<Sorting> Sortings { get; set; } = new List<Sorting>();
    }
}