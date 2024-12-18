using CapstonePrototype.Dto;
using CapstonePrototype.Dto.Rfq;
using SealBackend.Dto;

namespace CapstonePrototype.Services.RfqService;
public interface IRfqService
{
    public Task<ServiceResponse<RfqResponseDto>> CreateRfq(RfqInputDto rfq);
    public Task<ServiceResponse<TableDto<RfqItemDto>>> GetRfqTable(int pageSize,int page,string category,int? userId=null);
    public Task<ServiceResponse<TableDto<RfqItemDto>>> GetRfqTableBasedAuthCat(int pageSize, int page);
    public Task<ServiceResponse<TableDto<RfqItemDto>>> SearchRfqTable(int pageSize,int page,string search);
    public Task<ServiceResponse<RfqResponseDto>> GetRfqDetail(int rfqId);
}