using System.Collections.Generic;

namespace Filtr.Models.Base
{
    /// <summary> Contains options for filtering </summary>
    public class BaseFilterRequest<TFilterDto> where TFilterDto : class
    {
        /// <summary> Dto that contains data to filter </summary>
        public TFilterDto FilterDto { get; set; }

        /// <summary> Sortings </summary>
        public Sorting[] Sortings { get; set; } = new Sorting[0];
    }
}
