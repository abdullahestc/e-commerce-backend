using ECommerceAPI.DTOs;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ECommerceAPI.Common;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductsAsync();
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductAsync(id);
            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var result = await _productService.CreateProductAsync(productDto);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return HandleResult(result);
        }


        private IActionResult HandleResult<T>(ServiceResult<T> result)
        {
            if (result.Success)
            {
                return result.Data != null ? Ok(result) : NoContent();
            }
            return BadRequest(result);
        }

        private IActionResult HandleResult(ServiceResult result)
        {
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}