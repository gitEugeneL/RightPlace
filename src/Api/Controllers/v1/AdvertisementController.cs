using Application.Common.Models;
using Application.Operations.Advertisements;
using Application.Operations.Advertisements.Commands.CreateAdvertisement;
using Application.Operations.Advertisements.Commands.DeleteAdvertisement;
using Application.Operations.Advertisements.Commands.UpdateAdvertisement;
using Application.Operations.Advertisements.Queries.GetAdvertisement;
using Application.Operations.Advertisements.Queries.GetAllAdvertisements;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/advertisements")]
public class AdvertisementController : BaseController
{
    public AdvertisementController(IMediator mediator) : base(mediator) { }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(AdvertisementResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AdvertisementResponse>> Create([FromBody] CreateAdvertisementCommand command)
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
    [ProducesResponseType(typeof(AdvertisementResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdvertisementResponse>> Update([FromBody] UpdateAdvertisementCommand command)
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

        var command = new DeleteAdvertisementCommand { AdvertisementId = advertisementId }
            .SetCurrentUserId(userId);
        
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdvertisementResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AdvertisementResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetAdvertisementQuery(id));
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<AdvertisementResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<AdvertisementResponse>>> 
        GetAll([FromQuery] GetAllAdvertisementsQueryWithPagination query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}
