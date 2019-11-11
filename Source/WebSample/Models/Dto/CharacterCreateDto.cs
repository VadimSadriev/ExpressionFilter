using WebSample.Data.Enums;

namespace WebSample.Models.Dto
{
    public class CharacterCreateDto
    {
        public string Name { get; set; }

        public CharacterType Type { get; set; }
    }
}
