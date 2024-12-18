using CapstonePrototype.Data;
using CapstonePrototype.Dto.Company;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.CompanyService;
public class CompanyService(ApplicationDbContext context):ICompanyService
{
    private readonly ApplicationDbContext _context = context;
    public async Task<ServiceResponse<List<CompanyItemResultDto>>> GetCompanyIL()
    {
        try
        {
            var companyList = await _context.Companies
            .Select(c=>new CompanyItemResultDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
            return new ServiceResponse<List<CompanyItemResultDto>>
            {
                Data = companyList
            };
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return new ServiceResponse<List<CompanyItemResultDto>>
            {
                Success = false,
                Message = "Error when getting company",
                Data = null
            };
        }
    }
}