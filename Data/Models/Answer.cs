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
        public DateTime PublishedDate { get; set; }
    }
}
