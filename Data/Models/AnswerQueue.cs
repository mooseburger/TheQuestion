using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    public class AnswerQueue : AnswerBase
    {
        [Required]
        [Column(Order = 1)]
        public int AnswerStatusId { get; set; }

        [Required]
        [Column(Order = 10)]
        public DateTime ModifiedDate { get; set; }
    }
}
