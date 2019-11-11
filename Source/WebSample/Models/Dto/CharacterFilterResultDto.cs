using System.Collections.Generic;
using WebSample.Data.Entities;

namespace WebSample.Models.Dto
{
    public class CharacterFilterResultDto
    {
        public long TotalCount { get; set; }

        public decimal TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public ICollection<Character> Items { get; set; }
    }
}
