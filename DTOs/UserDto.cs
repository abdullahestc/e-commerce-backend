using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class UserDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        [StringLength(11)]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } 

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}