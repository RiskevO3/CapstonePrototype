using CapstonePrototype.Dto.RfqBid;
using SealBackend.Dto;

namespace CapstonePrototype.Services.RfqBidService;
public interface IRfqBidService
{
    public Task<ServiceResponse<bool>> CreateRfqBid(RfqBidInputDto rfqBid);
    public Task<ServiceResponse<List<RfqBidItemResponseDto>>> GetRfqBids();
    public Task<ServiceResponse<List<RfqBidItemResponseDto>>> GetRfqCompanyBids();
    public Task<ServiceResponse<RfqBidInputDto>> GetRfqBidDetail(int id);
    public Task<ServiceResponse<bool>> ChangeRfqBidStatus(RfqBidStatusInputDto input);
    public Task<ServiceResponse<bool>> UploadInvoiceOrResi(RfqBidInputInvoiceOrResidto input);
    public Task<ServiceResponse<bool>> UploadImage(RfqBidInputPOPDto input);
    public Task<ServiceResponse<bool>> CompleteRfqBid(int rfqBidId);
}