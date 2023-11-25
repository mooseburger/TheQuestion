using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.User
{
    public class User
    {
        [Required]
        [StringLength(256)]
        [DisplayName("Username")]
        public string? Username { get; set; }

        [Required]
        [StringLength(256)]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayName("Role")]
        public string? RoleName { get; set; }

        public List<SelectListItem>? Roles { get; set; } = null;

        public IEnumerable<IdentityError>? Errors { get; set; } = null;

        public void SetRoles(List<IdentityRole> identityRoles)
        {
            Roles = identityRoles.Select(r => new SelectListItem() { Value = r.Name, Text = r.NormalizedName }).ToList();
        }
    }
}
