using System.ComponentModel.DataAnnotations;

namespace TheQuestion.Model.Admin
{
    public class User
    {
        [Required]
        [StringLength(256)]
        public string? Username { get; set; }

        [Required]
        [StringLength(256)]
        public string? Email { get; set; }

        [StringLength(256)]
        public string? RoleName { get; set; }
    }
}
