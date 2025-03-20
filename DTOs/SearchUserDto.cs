using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class SearchUserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
    }
}