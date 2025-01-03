using CapstonePrototype.Data;
using CapstonePrototype.Dto.RfqBid;
using CapstonePrototype.Models;
using CapstonePrototype.Services.AuthService;
using CapstonePrototype.Services.FileUploadService;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SealBackend.Dto;

namespace CapstonePrototype.Services.RfqBidService;
public class RfqBidService(ApplicationDbContext context,IAuthService authService,IFileUploadService fileUploadService):IRfqBidService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IAuthService _authService = authService;
    private readonly IFileUploadService _fileUploadService = fileUploadService;

    public async Task<ServiceResponse<bool>> CreateRfqBid(RfqBidInputDto rfqBid)
    {
        try
        {
            var authUser = await _authService.GetAuthenticatedUser();
            if(authUser == null) return new ServiceResponse<bool>{Data = false, Message = "User not found", Success = false};
            var isRfqExist = await _context.Rfqs.FirstOrDefaultAsync(r => r.Id == rfqBid.RfqId && r.UserId != authUser.Id);
            if(isRfqExist == null) return new ServiceResponse<bool>{Data = false, Message = "RFQ not found", Success = false};
            var newRfqBid = new RfqBid
            {
                RfqId = rfqBid.RfqId,
                CompanyId = authUser.CompanyId,
                UserId = authUser.Id,
                BidStatus = "Pending",
                Description = rfqBid.Description,
                Amount = rfqBid.Amount,
                OrderDeadline = rfqBid.OrderDeadline,
                ExpectedArrival = rfqBid.ExpectedArrival,
            };
            await _context.RfqBids.AddAsync(newRfqBid);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>{Data = true, Message = "RFQ Bid created successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Error creating RFQ Bid",
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<List<RfqBidItemResponseDto>>> GetRfqBids()
    {
        try
        {
            var authUser = await _authService.GetAuthenticatedUser();
            if(authUser == null) return new ServiceResponse<List<RfqBidItemResponseDto>>{Data = null, Message = "User not found", Success = false};
            var rfqBidList = await _context.RfqBids
                .Include(r => r.Rfq)
                .Include(c => c.Company)
                .Where(r => r.UserId == authUser.Id)
                .Select(r => new RfqBidItemResponseDto
                {
                    Id = r.Id,
                    Name = r.Rfq.Title,
                    CompanyName = r.Rfq.Company.Name,
                    Category = r.Rfq.CompCategory.Name,
                    Status = r.BidStatus,
                    OrderDeadline = r.OrderDeadline,
                    ExpectedArrival = r.ExpectedArrival,
                    BidStatus = r.BidStatus,
                    Amount = r.Amount
                }).ToListAsync();
            return new ServiceResponse<List<RfqBidItemResponseDto>>{Data = rfqBidList, Message = "RFQ Bids retrieved successfully", Success = true};

        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<List<RfqBidItemResponseDto>>
            {
                Data = null,
                Message = "Error getting RFQ Bids",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<List<RfqBidItemResponseDto>>> GetRfqCompanyBids()
    {
        try
        {
            var authUser = await _authService.GetAuthenticatedUser();
            var compUser = await _context.Users.Select(u=>new{
                u.Id,
                CompanyId = u.Company.Id
            })
            .FirstOrDefaultAsync(u=>u.Id == authUser.Id);
            if(authUser == null || compUser == null) return new ServiceResponse<List<RfqBidItemResponseDto>>{Data = null, Message = "User not found", Success = false};
            var rfqBidList = await _context.RfqBids
                .Include(r => r.Rfq)
                .ThenInclude(c=>c.Company)
                .Include(c => c.Company)
                .Where(r => r.Rfq.Company.Id == compUser.CompanyId)
                .Select(r => new RfqBidItemResponseDto
                {
                    Id = r.Id,
                    Name = r.Rfq.Title,
                    CompanyName = r.Company.Name,
                    Category = r.Rfq.CompCategory.Name,
                    OrderDeadline = r.OrderDeadline,
                    ExpectedArrival = r.ExpectedArrival,
                    Status = r.BidStatus,
                    BidStatus = r.BidStatus,
                    Amount = r.Amount
                }).ToListAsync();
            return new ServiceResponse<List<RfqBidItemResponseDto>>{Data = rfqBidList, Message = "RFQ Bids retrieved successfully", Success = true};

        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<List<RfqBidItemResponseDto>>
            {
                Data = null,
                Message = "Error getting RFQ Bids",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<RfqBidInputDto>> GetRfqBidDetail(int id)
    {
        try
        {
            // TODO:unsecured
            var rfqBidExist = await _context.RfqBids.FirstOrDefaultAsync(r => r.Id == id);
            if(rfqBidExist == null) return new ServiceResponse<RfqBidInputDto>{Data = null, Message = "RFQ Bid not found", Success = false};
            var rfqBidDetail = new RfqBidInputDto
            {
                RfqId = rfqBidExist.RfqId,
                Amount = rfqBidExist.Amount,
                Status = rfqBidExist.BidStatus,
                Description = rfqBidExist.Description,
                OrderDeadline = rfqBidExist.OrderDeadline,
                FileUrl = rfqBidExist.FileUrl,
                FilePaymentUrl = rfqBidExist.FilePaymentUrl,
                Resi = rfqBidExist.NoResi,
                IsCompleted = rfqBidExist.IsCompleted,
                ExpectedArrival = rfqBidExist.ExpectedArrival
            };
            return new ServiceResponse<RfqBidInputDto>{Data = rfqBidDetail, Message = "RFQ Bid retrieved successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<RfqBidInputDto>
            {
                Data = null,
                Message = "Error getting RFQ Bid",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<bool>> ChangeRfqBidStatus(RfqBidStatusInputDto input)
    {
        try
        {
            var isRBExist = await _context.RfqBids.FirstOrDefaultAsync(r=>r.Id==input.RfqBidId);
            if(isRBExist == null) return new ServiceResponse<bool>{Data = false, Message = "RFQ Bid not found", Success = false};
            isRBExist.BidStatus = input.Status;
            _context.RfqBids.Update(isRBExist);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>{Data = true, Message = "RFQ Bid status changed successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Error changing RFQ Bid status",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<bool>> UploadInvoiceOrResi(RfqBidInputInvoiceOrResidto input)
    {
        try
        {
            var isRBExist = await _context.RfqBids.FirstOrDefaultAsync(r=>r.Id==input.RfqBidId);
            if(isRBExist == null) return new ServiceResponse<bool>{Data = false, Message = "RFQ Bid not found", Success = false};
            if(input.FileUrl != "")
            {
                var fileUrl = await _fileUploadService.UploadPdfAsync("", input.FileUrl);
                isRBExist.FileUrl = fileUrl;
            }
            if(input.Resi != "")
            {
                isRBExist.NoResi = input.Resi;
            }
            _context.RfqBids.Update(isRBExist);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>{Data = true, Message = "Invoice or resi uploaded successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Error uploading invoice or resi",
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<bool>> UploadImage(RfqBidInputPOPDto input)
    {
        try
        {
            var isRBExist = await _context.RfqBids.FirstOrDefaultAsync(r=>r.Id==input.RfqBidId);
            if(isRBExist == null) return new ServiceResponse<bool>{Data = false, Message = "RFQ Bid not found", Success = false};
            var fileUrl = await _fileUploadService.UploadImageAsync("", input.ImageUrl);
            isRBExist.FilePaymentUrl = fileUrl;
            _context.RfqBids.Update(isRBExist);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>{Data = true, Message = "Image uploaded successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Error uploading image",
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<bool>> CompleteRfqBid(int rfqBidId)
    {
        try
        {
            var isRBExist = await _context.RfqBids.FirstOrDefaultAsync(r=>r.Id==rfqBidId);
            if(isRBExist == null) return new ServiceResponse<bool>{Data = false, Message = "RFQ Bid not found", Success = false};
            isRBExist.IsCompleted = true;
            _context.RfqBids.Update(isRBExist);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>{Data = true, Message = "RFQ Bid completed successfully", Success = true};
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error from RfqBidService: {e}");
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Error completing RFQ Bid",
                Success = false
            };
        }
    }
}