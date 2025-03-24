namespace ECommerceAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPrice { get; set; }
        public string ProductGroup { get; set; }
        public string ProductPicture { get; set; }
    }
}