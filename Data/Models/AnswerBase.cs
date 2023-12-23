using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    public abstract class AnswerBase
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Required]
        [MaxLength(2000)]
        [Column(Order = 2)]
        public string Text { get; set; } = string.Empty;

        [Required]
        [Column(Order = 3)]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
