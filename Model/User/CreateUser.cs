using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Model.User
{
    public class CreateUser : User
    {
        [Required]
        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [Required]
        [MinLength(12)]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}
