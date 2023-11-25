using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace TheQuestion.Model.Auth
{
    public class ChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IEnumerable<IdentityError>? Errors { get; set; } = null;
    }
}
