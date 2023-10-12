using AutoMapper;
using Communications.Api.ViewModels.Classifications;
using Communications.Api.ViewModels.Pagination;
using Communications.Application.Classifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedLib.Redis.Attributes;

namespace Communications.Api.Controllers.Classifications;

[Route("api/classifications/authorized")]
[ApiController]
public class AuthorizedClassificationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthorizedClassificationsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Cached]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync(short pageNumber, byte pageSize)
    {
        var getAllClassificationsQuery = new GetAllClassificationsQuery()
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var classifications = await _mediator.Send(getAllClassificationsQuery);
        var classificationsViewModel = _mapper.Map<PageViewModel<ClassificationViewModel>>(classifications);

        return Ok(classificationsViewModel);
    }
}