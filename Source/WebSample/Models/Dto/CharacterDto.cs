using WebSample.Data.Entities;

namespace WebSample.Models.Dto
{
    /// <summary>
    /// Dto for <see cref="Character"/>
    /// </summary>
    public class CharacterDto
    {
        /// <summary>
        /// Character identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Character name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dto for <see cref="Character"/>
        /// </summary>
        public CharacterDto() { }

        /// <summary>
        /// Dto for <see cref="Character"/>
        /// </summary>
        public CharacterDto(Character character)
        {
            Id = character.Id;
            Name = character.Name;
        }
    }
}
