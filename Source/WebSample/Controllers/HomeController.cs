using Filtr.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSample.Data.Enums;
using WebSample.Models.Dto;
using WebSample.Services.Interfaces;

namespace WebSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICharacterService _characterService;

        public HomeController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet]
        public IActionResult GetSecondaryData()
        {
            var characterTypes = Enum.GetValues(typeof(CharacterType)).Cast<CharacterType>()
                                  .Select(x => new { value = x.ToString() });

            return Ok(characterTypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharacter([FromBody]CharacterCreateDto dto)
        {
            var character = await _characterService.CreateAsync(dto);

            var characterDto = new CharacterDto(character);

            return Ok(characterDto);
        }

        [HttpGet]
        public async Task<IActionResult> FilterCharacters([FromBody]PagedFilterRequest<CharacterFilterDto> filterRequest)
        {
            var result = await _characterService.FilterAsync(filterRequest);

            return Ok(result);
        }
    }
}
