using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Models.User
{
    public class EditUser : User
    {
        public EditUser() { }

        public EditUser(IdentityUser user) 
        { 
            Username = user.UserName;
            OriginalUsername = user.UserName;
            Email = user.Email;
            Lockout = user.LockoutEnd.HasValue;
        }

        public string? OriginalUsername { get; set; }

        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }

        [DisplayName("Lockout")]
        public bool Lockout { get; set; }
    }
}
