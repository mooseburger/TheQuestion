using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class EditAnswer : CreateAnswer
    {
        [Required]
        public int Id { get; set; }
    }
}
