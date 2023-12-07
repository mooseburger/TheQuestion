using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class CreateAnswer
    {
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }

        public List<SelectListItem>? Statuses { get; set; } = null;

        public void SetStatuses(IEnumerable<AnswerStatusDto> statuses)
        {
            Statuses = statuses.Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.Name }).ToList();
        }
    }
}
