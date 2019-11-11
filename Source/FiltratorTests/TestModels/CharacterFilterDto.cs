using System;
using WebSample.Data.Enums;

namespace FiltratorTests.TestModels
{
    public class CharacterFilterDto
    {
        public string Name { get; set; }

        public long? UniverseId { get; set; }

        public string UniverseName { get; set; }

        public bool? IsDeleted { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? CreateDateFrom { get; set; }

        public DateTime? CreateDateTo { get; set; }

        public CharacterType? Type { get; set; }
    }
}
