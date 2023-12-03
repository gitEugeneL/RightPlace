using Application.Common.Models;
using Application.Operations.Adverts;
using Application.Operations.Adverts.Commands.CreateAdvert;
using Application.Operations.Adverts.Commands.DeleteAdvert;
using Application.Operations.Adverts.Commands.UpdateAdvert;
using Application.Operations.Adverts.Queries.GetAdvert;
using Application.Operations.Adverts.Queries.GetAllAdverts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/advertisements")]
public class AdvertController : BaseController
{
    public AdvertController(IMediator mediator) : base(mediator) { }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AdvertsResponse>> Create([FromBody] CreateAdvertCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Created($"api/advertisements/{result.Id}", result);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdvertsResponse>> Update([FromBody] UpdateAdvertCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{advertisementId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(Guid advertisementId)
    {
        var userId = CurrentUserId();
        if (userId is null)
            return BadRequest();

        var command = new DeleteAdvertCommand { AdvertId = advertisementId }
            .SetCurrentUserId(userId);
        
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdvertsResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetAdvertQuery(id));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<AdvertsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<AdvertsResponse>>> 
        GetAll([FromQuery] GetAllAdvertsQueryWithPagination query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
