using Api.Helpers;
using Application.Operations.Information;
using Application.Operations.Information.Commands.CreateInformation;
using Application.Operations.Information.Commands.UpdateInformation;
using Application.Operations.Information.Queries.GetInformation;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route(("api/v{v:apiVersion}/information"))]
public class InformationController : BaseController
{
    public InformationController(IMediator mediator) : base(mediator) { }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPost]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<InformationResponse>> Create([FromBody] CreateInformationCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Created($"api/information/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<InformationResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetInformationQuery(id));
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<InformationResponse>> Update([FromBody] UpdateInformationCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
