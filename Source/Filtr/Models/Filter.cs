using System.Collections.Generic;

namespace Filtr.Models
{
    /// <summary> Poco for filter name and data </summary>
    public class Filter
    {
        /// <summary> Property name to filter </summary>
        public string Name { get; set; }

        /// <summary> Values for property </summary>
        public string[] Values { get; set; } = new string[0];
    }
}