using CapstonePrototype.Data;
using CapstonePrototype.Dto.Product;
using CapstonePrototype.Models;
using CapstonePrototype.services.ImageService;
using CapstonePrototype.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.ProductService;
public class ProductService(ApplicationDbContext context, IImageService imageService,IAuthService authService) : IProductService
{
    private readonly IImageService _imageService = imageService;
    private readonly IAuthService _authService = authService;
    private readonly ApplicationDbContext _context = context;
    public async Task<ServiceResponse<ProductDto>> CreateNewProduct(ProductInsertDto product)
    {
        try
        {
            var user = await _authService.GetAuthenticatedUser();
            if (user == null) return new ServiceResponse<ProductDto> { Data = null, Message = "User not found", Success = false };
            var isProductExist = await _context.Products.FirstOrDefaultAsync(x => x.Name == product.Name && x.CompanyId == user.Company.Id);
            if (isProductExist != null) return new ServiceResponse<ProductDto> { Data = null, Message = "Product already exist", Success = false };
            var imageUrl = await _imageService.UploadImage(product.Image, "products");
            if (!imageUrl.Success) return new ServiceResponse<ProductDto> { Data = null, Message = "Image is invalid", Success = false };
            var newProduct = new Product
            {
                ImageUrl = imageUrl.Message,
                Name = product.Name,
                Description = product.Description,
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return new ServiceResponse<ProductDto> { Data = newProduct.AsDto(), Message = "Product created successfully", Success = true };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error from ProductService: {ex.Message}");
            return new ServiceResponse<ProductDto>
            {
                Data = null,
                Message = "Terjadi kesalahan saat menambahkan produk",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<List<ProductDto>>> SearchProduct(string ProductInq)
    {
        try
        {
            if(string.IsNullOrEmpty(ProductInq)) return new ServiceResponse<List<ProductDto>>{Data = null, Message = "Product name is empty"};
            if(ProductInq.Length < 3) return new ServiceResponse<List<ProductDto>>{Data = null, Message = "Product name is too short"};
            var products = await _context.Products.Where(x => x.Name.Contains(ProductInq)).ToListAsync();
            return new ServiceResponse<List<ProductDto>>{Data = products.Select(x => x.AsDto()).ToList(), Message = "Product found"};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from SearchProduct: {e}");
            return new ServiceResponse<List<ProductDto>>{Data = null, Message = "Terjadi kesalahan saat mencari produk"};
        }
    }
}