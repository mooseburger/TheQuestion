namespace TheQuestion.Models.Generic
{
    public class PaginatedRequest
    {
        public int PageNumber { get; set; }
        public int PageSize {  get; set; } 

        public int Offset => (PageNumber - 1) * PageSize;
    }
}
