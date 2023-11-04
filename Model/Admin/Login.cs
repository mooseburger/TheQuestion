using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Model.Admin
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public SignInResult? SignInResult { get; set; }
    }
}
