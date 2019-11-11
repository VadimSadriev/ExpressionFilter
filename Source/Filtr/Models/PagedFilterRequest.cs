using Filtr.Models.Base;

namespace Filtr.Models
{
    /// <summary> Contains options for paged filtering </summary>
    public class PagedFilterRequest<TFilterDto> : BaseFilterRequest<TFilterDto> where TFilterDto : class
    {
        /// <summary> Number of page to get elements on </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary> Number of elements on page </summary>
        public int PageSize { get; set; } = 20;
    }
}