using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.DTOs
{
    public class ProductDto
    {
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductDescription { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductGroup { get; set; }

        [Required]
        [StringLength(50)]
        public string ProductPicture { get; set; } 
    }
}