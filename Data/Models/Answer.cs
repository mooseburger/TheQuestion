using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    [Table("Answers")]
    [Index(nameof(Slug), IsUnique = true)]
    public class Answer
    {
        [Key] 
        public int Id { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        [MaxLength(50)]
        
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; }

        public AnswerStatus? Status { get; set; }
    }
}
