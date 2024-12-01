using CapstonePrototype.Data;
using CapstonePrototype.Dto;
using CapstonePrototype.Dto.PurchasedProduct;
using CapstonePrototype.Dto.Rfq;
using CapstonePrototype.Models;
using CapstonePrototype.Services.AuthService;
using CapstonePrototype.Services.PurchasedProductService;
using Microsoft.EntityFrameworkCore;
using SealBackend.Dto;

namespace CapstonePrototype.Services.RfqService;
public class RfqService(ApplicationDbContext context,IPurchasedProductService purchasedProductService,IAuthService authService):IRfqService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IPurchasedProductService _purchasedProductService = purchasedProductService;
    private readonly IAuthService _authService = authService;
    public async Task<ServiceResponse<RfqResponseDto>> CreateRfq(RfqInputDto rfq)
    {
        try
        {
            var authUser = await _authService.GetAuthenticatedUser();
            if(authUser == null)return new ServiceResponse<RfqResponseDto>{Data = null,Message = "User not found",Success = false};
            var company = await _context.Companies.FirstOrDefaultAsync(c=>c.Id == authUser.Company.Id);
            if(company == null)return new ServiceResponse<RfqResponseDto>{Data = null,Message = "Company not found",Success = false};
            var compCategory = await _context.CompCategories.FirstOrDefaultAsync(c=>c.Id == rfq.CompCategoryId);
            if(compCategory == null)return new ServiceResponse<RfqResponseDto>{Data = null,Message = "Comp category not found",Success = false};
            var newRfq = new Rfq
            {
                Title = rfq.Title,
                Company = company,
                CompCategory = compCategory,
                BidType = rfq.BidType,
                Description = rfq.Description,
                OrderDeadline = rfq.OrderDeadline,
                ExpectedArrival = rfq.ExpectedArrival,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _context.Rfqs.AddAsync(newRfq);
            await _context.SaveChangesAsync();
            foreach(var purchProd in rfq.PurchasedProducts)
            {
                purchProd.RfqId = newRfq.Id;
                newRfq.Amount += purchProd.Amount * purchProd.Quantity;
            }
            var purchasedProduct = await _purchasedProductService.CreateBulk(rfq.PurchasedProducts);
            if(!purchasedProduct.Success || purchasedProduct.Data == null)return new ServiceResponse<RfqResponseDto>{Data = null,Message = "Failed to create purchased products",Success = false};
            List<PurchasedProductDto> purchasedProductDtos = purchasedProduct.Data;
            RfqResponseDto rfqResponse = newRfq.AsDto(purchasedProductDtos);
            return new ServiceResponse<RfqResponseDto>
            {
                Data = rfqResponse,
                Message = "RFQ berhasil dibuat",
                Success = true
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from CreateRfq: {e.Message}");
            return new ServiceResponse<RfqResponseDto>
            {
                Data = null,
                Message = "Terjadi kesalahan saat membuat RFQ",
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<TableDto<RfqItemDto>>> GetRfqTable(int pageSize,int page,string category)
    {
        try
        {
            if(pageSize > 100)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Page size is too large",Success = false};
            if(page < 1)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Page must be greater than 0",Success = false};
            if(category.Length < 3)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Category name is too short",Success = false};
            var categoryRes = await _context.CompCategories.FirstOrDefaultAsync(c=>c.Name == category);
            if(categoryRes == null)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Category not found",Success = false};
            var rfqs = await _context.Rfqs.Where(r=>r.CompCategory.Id == categoryRes.Id).Include(r=>r.Company).Include(r=>r.CompCategory).Skip(pageSize * (page - 1)).Take(pageSize).OrderByDescending(r=>r.CreatedAt).Select(r=>r.AsItemDto()).ToListAsync();
            var totalRfqs = await _context.Rfqs.Where(r=>r.CompCategory.Id == categoryRes.Id).CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalRfqs / pageSize);
            PaginationDto pagination = new()
            {
                Page = page,
                PageSize = pageSize,
                TotalData = totalRfqs,
                TotalPage = totalPage
            };
            TableDto<RfqItemDto> table = new()
            {
                Data = rfqs,
                Pagination = pagination
            };
            return new ServiceResponse<TableDto<RfqItemDto>>
            {
                Data = table,
                Message = "Berhasil mengambil data RFQ",
                Success = true
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from GetRfqTable: {e.Message}");
            return new ServiceResponse<TableDto<RfqItemDto>>
            {
                Data = null,
                Message = "Terjadi kesalahan saat mengambil data RFQ",
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<TableDto<RfqItemDto>>> SearchRfqTable(int pageSize,int page,string search)
    {
        try
        {
            if(search.Trim().Length < 3)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Search query is too short",Success = false};
            if(pageSize > 100)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Page size is too large",Success = false};
            if(page < 1)return new ServiceResponse<TableDto<RfqItemDto>>{Data = null,Message = "Page must be greater than 0",Success = false};
            var rfqs = await _context.Rfqs.Where(r=>r.Title.Contains(search)).Include(r=>r.Company).Include(r=>r.CompCategory).Skip(pageSize * (page - 1)).Take(pageSize).OrderByDescending(r=>r.CreatedAt).Select(r=>r.AsItemDto()).ToListAsync();
            var totalRfqs = await _context.Rfqs.Where(r=>r.Title.Contains(search)).CountAsync();
            var totalPage = (int)Math.Ceiling((double)totalRfqs / pageSize);
            PaginationDto pagination = new()
            {
                Page = page,
                PageSize = pageSize,
                TotalData = totalRfqs,
                TotalPage = totalPage
            };
            TableDto<RfqItemDto> table = new()
            {
                Data = rfqs,
                Pagination = pagination
            };
            return new ServiceResponse<TableDto<RfqItemDto>>
            {
                Data = table,
                Message = "Berhasil mencari data RFQ",
                Success = true
            };
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from SearchRfqTable: {e.Message}");
            return new ServiceResponse<TableDto<RfqItemDto>>
            {
                Data = null,
                Message = "Terjadi kesalahan saat mencari data RFQ",
                Success = false
            };
        }
    }
}