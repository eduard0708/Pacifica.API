using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Services.ProductService
{
    public interface IProductService
    {
        Task<ApiResponse<IEnumerable<Product>>> GetAllProductsAsync();
        Task<ApiResponse<Product>> GetProductByIdAsync(int id);
        Task<ApiResponse<Product>> CreateProductAsync(Product product);
        Task<ApiResponse<Product>> UpdateProductAsync(int id, Product product);
        Task<ApiResponse<bool>> DeleteProductAsync(int id);
    }
}