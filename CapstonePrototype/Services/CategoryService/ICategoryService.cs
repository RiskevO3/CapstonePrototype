using CapstonePrototype.Dto.Category;
using SealBackend.Dto;

namespace CapstonePrototype.Services.CategoryService;
public interface ICategoryService
{
    public Task<ServiceResponse<List<CategoryResultDto>>> SearchCategory(string query);
}