using CapstonePrototype.Dto;
using CapstonePrototype.Dto.Rfq;
using CapstonePrototype.Services.RfqService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealBackend.Dto;

namespace CapstonePrototype.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RfqController(IRfqService rfqService):ControllerBase
{
    private readonly IRfqService _rfqService = rfqService;

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<RfqResponseDto>>> AddRfq(RfqInputDto rfq)
    {
        var response = await _rfqService.CreateRfq(rfq);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("rfqs")]
    public async Task<ActionResult<ServiceResponse<TableDto<RfqItemDto>>>> GetRfqs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string category = ""
    )
    {
        var response = await _rfqService.GetRfqTable(page, pageSize, category);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("rfqsearch")]
    public async Task<ActionResult<ServiceResponse<TableDto<RfqItemDto>>>> SearchRfq(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = ""
    )
    {
        var response = await _rfqService.SearchRfqTable(page, pageSize, search);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }
}