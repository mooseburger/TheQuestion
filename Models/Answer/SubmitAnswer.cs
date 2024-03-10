using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class SubmitAnswer
    {
        [Required(ErrorMessage = "Please submit an answer...")]
        [MaxLength(2000)]
        [DisplayName("Answer")]
        public string Answer { get; set; } = string.Empty;

        [Required]
        [DisplayName("CAPTCHA")]
        public string CaptchaToken { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
