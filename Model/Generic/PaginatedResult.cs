using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TheQuestion.Model.Generic
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Page { get; set; } = Enumerable.Empty<T>();
        public int TotalRecords { get; set; }
    }
}
