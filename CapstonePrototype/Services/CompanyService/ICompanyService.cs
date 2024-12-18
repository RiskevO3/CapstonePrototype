using CapstonePrototype.Dto.Company;
using SealBackend.Dto;

namespace CapstonePrototype.Services.CompanyService;
public interface ICompanyService
{
    public Task<ServiceResponse<List<CompanyItemResultDto>>> GetCompanyIL();
}