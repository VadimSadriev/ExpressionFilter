using Filtr.Extensions;
using Filtr.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSample.Data;
using WebSample.Data.Entities;
using WebSample.Models.Dto;
using WebSample.Services.Interfaces;

namespace WebSample.Services
{
    /// <summary>
    /// Business logic related to <see cref="Character"/>
    /// </summary>
    public class CharacterService : ICharacterService
    {
        /// <summary>
        /// main data provider
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Business logic related to <see cref="Character"/>
        /// </summary>
        public CharacterService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tries to create character from given <see cref="CharacterDto"/>
        /// </summary>
        public async Task<Character> CreateAsync(CharacterCreateDto characterDto)
        {
            try
            {
                if (_context.Characters.Any(x => x.Name.ToLower() == characterDto.Name.ToLower()))
                    throw new Exception("Character with given name already exists");

                var character = new Character
                {
                    Name = characterDto.Name
                };

                await _context.AddAsync(character);

                await _context.SaveChangesAsync();

                return await _context.Characters.FirstOrDefaultAsync(x => x.Id == character.Id);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Returns list of existing characters
        /// </summary>
        public async Task<CharacterFilterResultDto> FilterAsync(PagedFilterRequest<CharacterFilterDto> filterRequest)
        {
            var filteredQuery = _context.Characters.Filter(filterRequest);

            var totalCount = await filteredQuery.CountAsync();

            var items = await filteredQuery
                .Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
                .Take(filterRequest.PageSize)
                .ToListAsync();

            var totalPages = Math.Ceiling((decimal)totalCount / filterRequest.PageSize);

            return new CharacterFilterResultDto
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = filterRequest.PageNumber > 1,
                HasNextPage = filterRequest.PageNumber < totalPages,
                Items = items
            };
        }
    }
}
