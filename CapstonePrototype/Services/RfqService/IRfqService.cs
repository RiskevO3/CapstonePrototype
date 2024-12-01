using CapstonePrototype.Dto;
using CapstonePrototype.Dto.Rfq;
using SealBackend.Dto;

namespace CapstonePrototype.Services.RfqService;
public interface IRfqService
{
    public Task<ServiceResponse<RfqResponseDto>> CreateRfq(RfqInputDto rfq);
    public Task<ServiceResponse<TableDto<RfqItemDto>>> GetRfqTable(int pageSize,int page,string category);
    public Task<ServiceResponse<TableDto<RfqItemDto>>> SearchRfqTable(int pageSize,int page,string search);
}