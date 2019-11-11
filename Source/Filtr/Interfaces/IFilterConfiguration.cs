using Filtr.Models;

namespace Filtr.Interfaces
{
    /// <summary>
    /// Interface to implement for configuring filter setting
    /// </summary>
    /// <typeparam name="TFromEntity">Object type to bind from</typeparam>
    /// <typeparam name="TToEntity">Object type to bind to</typeparam>
    public interface IFilterConfiguration<TFromEntity, TToEntity>
        where TFromEntity : class
        where TToEntity : class
    {
        /// <summary>
        /// Configures filter settings
        /// </summary>
        void Configure(FilterBuilder<TFromEntity, TToEntity> builder);
    }
}
