using Mapster;
using ECommerceAPI.Models;
using ECommerceAPI.DTOs;

namespace ECommerceAPI.Common
{
    public class MappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<UserDto, User>
                .NewConfig()
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.Mail, src => src.Mail)
                .Map(dest => dest.Phone, src => src.Phone)
                .Map(dest => dest.Role, src => src.Role)
                .Map(dest => dest.Password, src => src.Password);

            TypeAdapterConfig<ProductDto, Product>
                .NewConfig()
                .Map(dest => dest.ProductName, src => src.ProductName)
                .Map(dest => dest.ProductDescription, src => src.ProductDescription)
                .Map(dest => dest.ProductPrice, src => src.ProductPrice)
                .Map(dest => dest.ProductGroup, src => src.ProductGroup)
                .Map(dest => dest.ProductPicture, src => src.ProductPicture);
        }
    }
}
