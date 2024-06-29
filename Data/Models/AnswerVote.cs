using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheQuestion.Data.Models
{
    [Table("AnswerVotes")]
    public class AnswerVote
    {
        [Column(Order = 0)]
        public int AnswerId { get; set; }

        [Column(Order = 1)]
        [MaxLength(50)]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        [Column(Order = 3)]
        public DateTimeOffset VoteDate { get; set; }
    }
}
