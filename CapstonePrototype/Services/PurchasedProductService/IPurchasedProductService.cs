using CapstonePrototype.Dto.PurchasedProduct;
using SealBackend.Dto;

namespace CapstonePrototype.Services.PurchasedProductService;
public interface IPurchasedProductService
{
    public Task<ServiceResponse<PurchasedProductDto>> Create(PurchasedProductInsertDto product);
    public Task<ServiceResponse<List<PurchasedProductDto>>> CreateBulk(List<PurchasedProductInsertDto> products);
}