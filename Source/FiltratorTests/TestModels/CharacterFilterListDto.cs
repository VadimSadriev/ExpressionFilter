using System.Collections.Generic;
using WebSample.Data.Enums;

namespace FiltratorTests.TestModels
{
    public class CharacterFilterListDto
    {
        public List<string> Names { get; set; }

        public List<long?> Ids { get; set; }

        public List<CharacterType> Types { get; set; }
    }
}
