using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class CreateAnswer
    {
        [Required]
        [MaxLength(2000)]
        [DisplayName("Text")]
        public string Text { get; set; } = string.Empty;
    }
}
