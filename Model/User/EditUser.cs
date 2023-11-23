using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Model.User
{
    public class EditUser : User
    {
        public EditUser(IdentityUser user) 
        { 
            Username = user.UserName;
            Email = user.Email;
        }

        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
