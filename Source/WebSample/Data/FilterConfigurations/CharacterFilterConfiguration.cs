using Filtr.Enums;
using Filtr.Interfaces;
using Filtr.Models;
using WebSample.Data.Entities;
using WebSample.Models.Dto;

namespace WebSample.Data.FilterConfigurations
{
    public class CharacterFilterConfiguration : IFilterConfiguration<CharacterFilterDto, Character>
    {
        public void Configure(FilterBuilder<CharacterFilterDto, Character> builder)
        {
            builder.Map(x => x.Name, x => x.Name, OperatorBetween.None, Method.Contains);

            builder.Default(x => x.Id);
        }
    }
}
