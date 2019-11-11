using System;
using WebSample.Data.Enums;

namespace WebSample.Data.Entities
{
    /// <summary> Base class for every character </summary>
    public class Character
    {
        /// <summary> character identifier </summary>
        public long Id { get; set; }

        /// <summary> character name </summary>
        public string Name { get; set; }

        /// <summary> character creating date </summary>
        public DateTime DateCreated { get; set; }

        /// <summary> character modified date </summary>
        public DateTime DateModified { get; set; }

        /// <summary> Character type </summary>
        public CharacterType Type { get; set; }

        public bool? IsDeleted { get; set; }

        /// <summary> Universe identifier </summary>
        public long? UniverseId { get; set; }

        /// <summary> Navigation property to universe </summary>
        public Universe Universe { get; set; }
    }
}
