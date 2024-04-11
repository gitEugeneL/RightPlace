using Api.Helpers;
using Application.Common.Models;
using Application.Operations.Adverts;
using Application.Operations.Adverts.Commands.CreateAdvert;
using Application.Operations.Adverts.Commands.DeleteAdvert;
using Application.Operations.Adverts.Commands.UpdateAdvert;
using Application.Operations.Adverts.Queries.GetAdvert;
using Application.Operations.Adverts.Queries.GetAllAdverts;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/advertisements")]
public class AdvertController : BaseController
{
    public AdvertController(IMediator mediator) : base(mediator) { }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPost]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AdvertsResponse>> Create([FromBody] CreateAdvertCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Created($"api/advertisements/{result.Id}", result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AdvertsResponse>> Update([FromBody] UpdateAdvertCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpDelete("{advertisementId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(Guid advertisementId)
    {
        var command = new DeleteAdvertCommand { AdvertId = advertisementId }
            .SetCurrentUserId(CurrentUserId());
        
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdvertsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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
