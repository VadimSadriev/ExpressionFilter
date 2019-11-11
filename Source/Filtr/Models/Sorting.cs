using Filtr.Enums;

namespace Filtr.Models
{
    /// <summary> Poco for sorting data </summary>
    public class Sorting
    {
        /// <summary> Property name to sort </summary>
        public string Name { get; set; }

        /// <summary> Property priority for sorting </summary>
        public int Priority { get; set; }

        /// <summary> Direction of sorting </summary>
        public SortingDirection Direction { get; set; }
    }
}