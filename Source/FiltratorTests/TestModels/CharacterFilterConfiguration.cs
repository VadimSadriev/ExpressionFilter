using Filtr.Enums;
using Filtr.Interfaces;
using Filtr.Models;
using FiltratorTests.TestModels;
using WebSample.Data.Entities;

namespace FiltratorTests
{
    public class CharacterFilterConfiguration : IFilterConfiguration<CharacterFilterDto, Character>
    {
        public void Configure(FilterBuilder<CharacterFilterDto, Character> builder)
        {
            builder.Map(x => x.Name, x => x.Name, OperatorBetween.None, Method.Contains)
                .Map(x => x.CreateDateFrom, x => x.DateCreated, OperatorBetween.None, Method.GreatherOrEqualThen)
                .Map(x => x.CreateDateTo, x => x.DateCreated, OperatorBetween.None, Method.LessOrEqualThen)
                .Map(x => x.CreateDate, x => x.DateCreated, OperatorBetween.None, Method.Equal)
                .Map(x => x.Type, x => x.Type, OperatorBetween.None, Method.Equal)
                .Map(x => x.UniverseId, x => x.UniverseId, OperatorBetween.None, Method.Equal)
                .Map(x => x.UniverseName, x => x.Universe.Name, OperatorBetween.None, Method.Contains)
                .Map(x => x.IsDeleted, x => x.IsDeleted, OperatorBetween.None, Method.Equal);

            builder.Default(x => x.IsDeleted);
        }
    }
}
