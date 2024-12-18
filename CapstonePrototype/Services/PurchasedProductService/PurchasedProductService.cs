using CapstonePrototype.Data;
using CapstonePrototype.Dto.PurchasedProduct;
using CapstonePrototype.Models;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.PurchasedProductService;
public class PurchasedProductService(ApplicationDbContext context):IPurchasedProductService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ServiceResponse<PurchasedProductDto>> Create(PurchasedProductInsertDto product)
    {
        try
        {
            var isProductExist = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId);
            if(isProductExist == null) return new ServiceResponse<PurchasedProductDto>{Data = null, Message = "Product not found", Success = false};
            var isRfqExist = await _context.Rfqs.FirstOrDefaultAsync(x => x.Id == product.RfqId);
            if(isRfqExist == null) return new ServiceResponse<PurchasedProductDto>{Data = null, Message = "Rfq not found", Success = false};
            PurchasedProduct purchasedProduct = new(){
                Product = isProductExist,
                Rfq = isRfqExist,
                UnitPrice = product.UnitPrice,
                Quantity = product.Quantity,
                Amount = product.Amount
            };
            await _context.PurchasedProducts.AddAsync(purchasedProduct);
            await _context.SaveChangesAsync();
            return new ServiceResponse<PurchasedProductDto>{Data = purchasedProduct.AsDto(), Message = "Produk berhasil ditambahkan ke keranjang", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from PurchasedProductService.Create: {e.Message}");
            return new ServiceResponse<PurchasedProductDto>{Success = false, Message = "Terjadi kesalahan saat menambahkan produk ke keranjang"};
        }
    }
    public async Task<ServiceResponse<List<PurchasedProductDto>>> CreateBulk(List<PurchasedProductInsertDto> products)
    {
        try
        {
            List<PurchasedProductDto> purchasedProducts = [];
            var isRfqExist = await _context.Rfqs.FirstOrDefaultAsync(x => x.Id == products[0].RfqId);
            if(isRfqExist == null) return new ServiceResponse<List<PurchasedProductDto>>{Data = null, Message = "Rfq not found", Success = false};
            foreach(var product in products)
            {
                var isProductExist = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId);
                if(isProductExist == null) return new ServiceResponse<List<PurchasedProductDto>>{Data = null, Message = "Product not found", Success = false};
                PurchasedProduct purchasedProduct = new(){
                    Product = isProductExist,
                    Rfq = isRfqExist,
                    UnitPrice = product.UnitPrice,
                    Quantity = product.Quantity,
                    Amount = product.UnitPrice * product.Quantity
                };
                await _context.PurchasedProducts.AddAsync(purchasedProduct);
                purchasedProducts.Add(purchasedProduct.AsDto());
            }
            await _context.SaveChangesAsync();
            return new ServiceResponse<List<PurchasedProductDto>>{Data = purchasedProducts, Message = "Produk berhasil ditambahkan ke keranjang", Success = true};
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error from PurchasedProductService.CreateBulk: {ex}");
            return new ServiceResponse<List<PurchasedProductDto>>{Success = false, Message = "Terjadi kesalahan saat menambahkan produk ke keranjang"};
        }
    }
}