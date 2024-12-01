using CapstonePrototype.Dto.Product;
using SealBackend.Dto;

namespace CapstonePrototype.Services.ProductService;
public interface IProductService
{
    public Task<ServiceResponse<List<ProductDto>>> SearchProduct(string ProductInq);
    public Task<ServiceResponse<ProductDto>> CreateNewProduct(ProductInsertDto product);
}