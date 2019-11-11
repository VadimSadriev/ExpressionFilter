using Filtr.Interfaces;
using Filtr.Models;
using WebSample.Data.Entities;

namespace FiltratorTests.TestModels
{
    public class CharacterFilterListConfiguration : IFilterConfiguration<CharacterFilterListDto, Character>
    {
        public void Configure(FilterBuilder<CharacterFilterListDto, Character> builder)
        {
            builder.Map(x => x.Names, x => x.Name, Filtr.Enums.OperatorBetween.Or, Filtr.Enums.Method.Contains)
                .Map(x => x.Ids, x => x.Id, Filtr.Enums.OperatorBetween.Or, Filtr.Enums.Method.Equal)
                .Map(x => x.Types, x => x.Type, Filtr.Enums.OperatorBetween.Or, Filtr.Enums.Method.Equal);

            builder.Default(x => x.Name);
        }
    }
}
