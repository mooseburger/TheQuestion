using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    [Table("AnswerStatuses")]
    public class AnswerStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;
    }

    public enum AnswerStatusEnum : int
    {
        InReview = 1,
        Rejected = 2
    }
}
