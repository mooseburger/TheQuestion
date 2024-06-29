using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    [Table("Answers")]
    public class Answer : AnswerBase
    {
        [Required]
        [Column(Order = 4)]
        public DateTimeOffset PublishedDate { get; set; }

        [Required]
        public double Rank { get; set; }

        [Required]
        public int TotalVotes { get; set; }
    }
}
