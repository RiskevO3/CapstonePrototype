using CapstonePrototype.Dto.RfqBid;
using CapstonePrototype.Services.RfqBidService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealBackend.Dto;

namespace CapstonePrototype.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RfqBidController(IRfqBidService rfqBidService):ControllerBase
{
    private readonly IRfqBidService _rfqBidService = rfqBidService;

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<bool>>> CreateRfqBid(RfqBidInputDto rfqBid)
    {
        var response = await _rfqBidService.CreateRfqBid(rfqBid);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("rfq-bid/{rfqBidId}")]
    public async Task<ActionResult<ServiceResponse<RfqBidInputDto>>> GetRfqBid(int rfqBidId)
    {
        var response = await _rfqBidService.GetRfqBidDetail(rfqBidId);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("rfq-company-bids")]
    public async Task<ActionResult<ServiceResponse<List<RfqBidItemResponseDto>>>> GetRfqBids()
    {
        var response = await _rfqBidService.GetRfqCompanyBids();
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpPost("change-status")]
    public async Task<ActionResult<ServiceResponse<bool>>> ChangeRfqBidStatus(RfqBidStatusInputDto input)
    {
        var response = await _rfqBidService.ChangeRfqBidStatus(input);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpPost("invoice-or-resid")]
    public async Task<ActionResult<ServiceResponse<bool>>> UploadInvoiceOrResid(RfqBidInputInvoiceOrResidto input)
    {
        var response = await _rfqBidService.UploadInvoiceOrResi(input);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpPost("upload-image")]
    public async Task<ActionResult<ServiceResponse<bool>>> UploadImage(RfqBidInputPOPDto input)
    {
        var response = await _rfqBidService.UploadImage(input);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpPatch("complete/{rfqBidId}")]
    public async Task<ActionResult<ServiceResponse<bool>>> CompleteRfqBid(int rfqBidId)
    {
        var response = await _rfqBidService.CompleteRfqBid(rfqBidId);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }
}