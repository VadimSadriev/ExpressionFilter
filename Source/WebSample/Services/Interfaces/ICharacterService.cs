using Filtr.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSample.Data.Entities;
using WebSample.Models.Dto;

namespace WebSample.Services.Interfaces
{
    /// <summary>
    /// Business logic related to <see cref="Character"/>
    /// </summary>
    public interface ICharacterService
    {
        /// <summary>
        /// Tries to create character from given <see cref="CharacterDto"/>
        /// </summary>
        Task<Character> CreateAsync(CharacterCreateDto characterDto);

        /// <summary>
        /// Returns list of existing characters
        /// </summary>
        /// <returns></returns>
        Task<CharacterFilterResultDto> FilterAsync(PagedFilterRequest<CharacterFilterDto> filterRequest);
    }
}
