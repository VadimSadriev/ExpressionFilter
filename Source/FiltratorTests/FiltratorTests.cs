using Filtr.Extensions;
using Filtr.Models;
using FiltratorTests.TestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using WebSample.Data;
using WebSample.Data.Entities;
using WebSample.Data.Enums;
using Filtrator = Filtr.Filtrator;

namespace FiltratorTests
{
    [TestClass]
    public class FiltratorTests
    {
        private DataContext _context;

        [TestInitialize]
        public void Init()
        {
            Filtrator.ClearSettings();

            // register filters
            Filtrator.ConfigureFilters(Assembly.GetExecutingAssembly());

            // setup context
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            FillContext();
        }

        /// <summary>
        /// Simple test to filter by name
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task FilterContainsTest()
        {
            var filterDto = new CharacterFilterDto
            {
                Name = "alex"
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasAlex = items.Any(x => x.Name == "Alex");

            Assert.IsTrue(hasAlex);
        }

        /// <summary>
        /// Test with list of names
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task FilterContainsListTest()
        {
            var filterDto = new CharacterFilterListDto
            {
                Names = new List<string>
                {
                    "diana",
                    "joHn"
                }
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterListDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasDianaJohn = items.Any(x => x.Name == "Diana" || x.Name == "John");

            Assert.IsTrue(hasDianaJohn);
        }

        /// <summary>
        /// Test for getting elements in date time interval
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task FilterDateTimeIntervalTest()
        {
            var filterDto = new CharacterFilterDto
            {
                CreateDateFrom = new DateTime(2008, 10, 15),
                CreateDateTo = DateTime.Now.AddYears(-2)
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasDianaAlex = items.Any(x => x.Name == "Alex" || x.Name == "Diana");

            Assert.IsTrue(hasDianaAlex);
        }

        /// <summary>
        /// test to filter just by datetime
        /// </summary>
        [TestMethod]
        public async Task FilterDatetimeTest()
        {
            var filterDto = new CharacterFilterDto
            {
                CreateDate = new DateTime(2010, 10, 5)
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var alex = items.FirstOrDefault(x => x.Name == "Alex");

            Assert.IsNotNull(alex);
        }

        /// <summary>
        /// Test to filter items by id list
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task FilterIdListTest()
        {
            var filterDto = new CharacterFilterListDto
            {
                Ids = new List<long?>
                {
                    2,
                    3
                }
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterListDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasDianaJohn = items.Any(x => x.Name == "Diana" || x.Name == "John");

            Assert.IsTrue(hasDianaJohn);
        }

        /// <summary>
        /// Get By concrete type test
        /// </summary>
        [TestMethod]
        public async Task GetByEnumTest()
        {
            var filterDto = new CharacterFilterDto
            {
                Type = CharacterType.Mage
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var gendalf = items.FirstOrDefault(x => x.Name == "Gendalf");

            Assert.IsNotNull(gendalf);
        }

        /// <summary>
        /// Get by list of types test
        /// </summary>
        [TestMethod]
        public async Task GetByEnumListTest()
        {
            var filterDto = new CharacterFilterListDto
            {
                Types = new List<CharacterType>
                {
                    CharacterType.Warrior,
                    CharacterType.Archer
                }
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterListDto>
            {
                FilterDto = filterDto,
                PageSize = 1,
                PageNumber = 1
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasLegolasAragorn = items.Any(x => x.Name == "Legolas" || x.Name == "Aragorn");

            Assert.IsTrue(hasLegolasAragorn);
        }

        [TestMethod]
        public async Task GetByForeignKey()
        {
            var filterDto = new CharacterFilterDto
            {
                UniverseId = 1
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            var hasMiddleEarthCharacters = items.Count == 2;

            Assert.IsTrue(hasMiddleEarthCharacters);
        }

        [TestMethod]
        public async Task GetByNullableBoolTest()
        {
            var filterDto = new CharacterFilterDto
            {
                IsDeleted = true
            };

            var pagedRequest = new PagedFilterRequest<CharacterFilterDto>
            {
                FilterDto = filterDto
            };

            var result = _context.Characters.Filter(pagedRequest);

            var items = await result.ToListAsync();

            Assert.IsNotNull(items.FirstOrDefault(x => x.Name == "I am deleted"));
        }

        private TOut GetPropertyValue<TEntity, TOut>(TEntity filter, Expression<Func<TEntity, TOut>> predicate)
        {
            var member = predicate.Body as MemberExpression;

            var entityConstant = Expression.Constant(filter);

            var propertyConstant = Expression.Constant("Kappa");

            var expr = Expression.Property(Expression.Constant(filter), member.Member.Name);

            var result = Expression.Lambda<Func<TOut>>(expr).Compile()();

            return result;
        }

        private void FillContext()
        {
            var alex = new Character
            {
                Name = "Alex",
                DateCreated = new DateTime(2010, 10, 5),
                DateModified = DateTime.Now
            };

            var deletedCharacter = new Character
            {
                Name = "I am deleted",
                DateCreated = new DateTime(2010, 10, 5),
                DateModified = DateTime.Now,
                IsDeleted = true
            };

            var john = new Character
            {
                Name = "John",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now
            };

            var diana = new Character
            {
                Name = "Diana",
                DateCreated = new DateTime(2010, 1, 1),
                DateModified = DateTime.Now
            };

            var middleEarth = new Universe
            {
                Name = "Middle Earth"
            };
            _context.Universes.Add(middleEarth);

            var legolas = new Character
            {
                Name = "Legolas",
                Type = CharacterType.Archer
            };

            var aragorn = new Character
            {
                Name = "Aragorn",
                Type = CharacterType.Warrior,
                UniverseId = middleEarth.Id
            };

            var frodo = new Character
            {
                Name = "Frodo",
                Type = CharacterType.Rogue
            };

            var gendalf = new Character
            {
                Name = "Gendalf",
                Type = CharacterType.Mage,
                UniverseId = middleEarth.Id
            };

            _context.Characters.Add(alex);
            _context.Characters.Add(diana);
            _context.Characters.Add(john);
            _context.Characters.Add(deletedCharacter);
            _context.Characters.Add(legolas);
            _context.Characters.Add(aragorn);
            _context.Characters.Add(frodo);
            _context.Characters.Add(gendalf);

            _context.SaveChanges();
        }
    }
}
