using CapstonePrototype.Data;
using CapstonePrototype.Dto.Category;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.CategoryService;
public class CategoryService(ApplicationDbContext context):ICategoryService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<ServiceResponse<List<CategoryResultDto>>> SearchCategory(string query)
    {
        try
        {
            if(query.Length < 3)return new ServiceResponse<List<CategoryResultDto>>{Data = null,Message = "Query terlalu pendek",Success = false};
            var categories = await _context.CompCategories.Where(c=>c.Name.Contains(query)).ToListAsync();
            return new ServiceResponse<List<CategoryResultDto>>
            {
                Data = categories.Select(c=>c.AsDto()).ToList(),
                Message = "Kategori berhasil ditemukan",
                Success = true
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from SearchCategory: {e.Message}");
            return new ServiceResponse<List<CategoryResultDto>>
            {
                Data = null,
                Message = "Terjadi kesalahan saat mencari kategori",
                Success = false
            };
        }
    }
}