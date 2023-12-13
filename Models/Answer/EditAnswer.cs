using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.Answer
{
    public class EditAnswer : CreateAnswer
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Status")]
        public int AnswerStatusId { get; set; }

        [DisplayName("Publish")]
        public bool Publish {  get; set; }

        public bool Next { get; set; }

        public DateTime CreatedDate { get; set; }

        public List<SelectListItem>? Statuses { get; set; } = null;

        public List<string>? Errors { get; set; }

        public void SetStatuses(IEnumerable<AnswerStatusDto> statuses)
        {
            Statuses = statuses.Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.Name }).ToList();
        }

    }
}
