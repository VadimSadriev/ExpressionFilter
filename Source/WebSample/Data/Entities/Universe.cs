using System.Collections.Generic;

namespace WebSample.Data.Entities
{
    /// <summary> Just universe where characters live </summary>
    public class Universe
    {
        /// <summary> Identifier of universe </summary>
        public long Id { get; set; }

        /// <summary> Name of the universe </summary>
        public string Name { get; set; }

        /// <summary> Characters who belong to this universe </summary>
        public ICollection<Character> Characters { get; set; }
    }
}
