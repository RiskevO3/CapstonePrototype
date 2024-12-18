using CapstonePrototype.Dto;
using CapstonePrototype.Dto.Category;
using CapstonePrototype.Dto.Rfq;
using CapstonePrototype.Dto.RfqBid;
using CapstonePrototype.Services.AuthService;
using CapstonePrototype.Services.CategoryService;
using CapstonePrototype.Services.RfqBidService;
using CapstonePrototype.Services.RfqService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SealBackend.Dto;

namespace CapstonePrototype.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RfqController(IRfqService rfqService,IAuthService authService,ICategoryService categoryService,IRfqBidService rfqBidService):ControllerBase
{
    private readonly IRfqService _rfqService = rfqService;
    private readonly IAuthService _authService = authService;
    private readonly ICategoryService _categoryService = categoryService;
    private readonly IRfqBidService _rfqBidService = rfqBidService;

    [HttpPost("add")]
    public async Task<ActionResult<ServiceResponse<RfqResponseDto>>> AddRfq(RfqInputDto rfq)
    {
        var response = await _rfqService.CreateRfq(rfq);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("owned-rfqs")]
    public async Task<ActionResult<ServiceResponse<TableDto<RfqItemDto>>>> GetOwnedRfqs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var authUser = await _authService.GetAuthenticatedUser();
        Console.WriteLine("Auth User: "+authUser);
        if(authUser == null)return Forbid();
        var response = await _rfqService.GetRfqTable(pageSize, page,"",authUser.Id);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("auth-category-rfqs")]
    public async Task<ActionResult<ServiceResponse<TableDto<RfqItemDto>>>> GetAuthCategoryRfqs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var response = await _rfqService.GetRfqTableBasedAuthCat(pageSize, page);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("rfq/{id}")]
    public async Task<ActionResult<ServiceResponse<RfqResponseDto>>> GetRfq(int id)
    {
        var response = await _rfqService.GetRfqDetail(id);
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
        var response = await _rfqService.GetRfqTable(pageSize, page, category);
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

    [HttpGet("owned-rfq-bids")]
    public async Task<ActionResult<ServiceResponse<List<RfqBidItemResponseDto>>>> GetOwnedRfqBids()
    {
        var response = await _rfqBidService.GetRfqBids();
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }

    [HttpGet("category-search")]
    public async Task<ActionResult<ServiceResponse<List<CategoryResultDto>>>> SearchCategory(
        [FromQuery] string search = ""
    )
    {
        var response = await _categoryService.SearchCategory(search);
        if(response.Success)return Ok(response);
        return BadRequest(response);
    }
}