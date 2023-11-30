using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Generic
{
    public class PaginatedRequest
    {
        public int PageNumber { get; set; }

        [Range(1, 100)]
        public int PageSize {  get; set; } 

        public int Offset => (PageNumber - 1) * PageSize;
    }
}
