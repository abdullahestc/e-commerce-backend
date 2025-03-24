using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceAPI.Common;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        Task<ServiceResult<Product>> CreateProductAsync(ProductDto productDto);
        Task<ServiceResult<Product>> GetProductAsync(int id);
        Task<ServiceResult<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ServiceResult<ProductDto>> UpdateProductAsync(int id, ProductDto productDto);
        Task<ServiceResult> DeleteProductAsync(int id);
    }
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
            MappingConfig.Configure();
        }

        public async Task<ServiceResult<Product>> CreateProductAsync(ProductDto productDto)
        {
            var productExists = await _context.Products
                .AnyAsync(p => p.ProductName == productDto.ProductName);
                
            if (productExists)
                return ServiceResult<Product>.ErrorResult(Messages.UsedProductName);

            var product = productDto.Adapt<Product>();
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return ServiceResult<Product>.SuccessResult(product, Messages.ProductAdded);
        }

        public async Task<ServiceResult<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            return product == null 
                ? ServiceResult<Product>.NotFoundResult(Messages.ProductNotFound) 
                : ServiceResult<Product>.SuccessResult(product);
        }

        public async Task<ServiceResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            
            return !products.Any()
                ? ServiceResult<IEnumerable<Product>>.NotFoundResult(Messages.ProductNotFound)
                : ServiceResult<IEnumerable<Product>>.SuccessResult(products);
        }

        public async Task<ServiceResult<ProductDto>> UpdateProductAsync(int id, ProductDto productDto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return ServiceResult<ProductDto>.NotFoundResult(Messages.ProductNotFound);

            var nameExists = await _context.Products
                .AnyAsync(p => p.ProductName == productDto.ProductName && p.Id != id);
                
            if (nameExists)
                return ServiceResult<ProductDto>.ErrorResult(Messages.UsedProductName);

            productDto.Adapt(existingProduct);
            await _context.SaveChangesAsync();

            return ServiceResult<ProductDto>.SuccessResult(productDto, Messages.ProductUpdated);
        }

        public async Task<ServiceResult> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ServiceResult.NotFoundResult(Messages.ProductNotFound);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return ServiceResult.SuccessResult(Messages.ProductDeleted);
        }
    }
}

