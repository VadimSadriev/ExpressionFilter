using Microsoft.EntityFrameworkCore;
using WebSample.Data.Entities;

namespace WebSample.Data
{
    /// <summary>Main data provider </summary>
    public class DataContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public DbSet<Universe> Universes { get; set; }

        /// <summary>Main data provider </summary>
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
