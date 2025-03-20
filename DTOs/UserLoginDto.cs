using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}