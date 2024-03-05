using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class SubmitAnswer : CreateAnswer
    {
        [Required]
        [DisplayName("CAPTCHA")]
        public string CaptchaToken { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
