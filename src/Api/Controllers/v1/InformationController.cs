using Application.Operations.Information;
using Application.Operations.Information.Commands.CreateInformation;
using Application.Operations.Information.Commands.UpdateInformation;
using Application.Operations.Information.Queries.GetInformation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route(("api/information"))]
public class InformationController : BaseController
{
    public InformationController(IMediator mediator) : base(mediator) { }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<InformationResponse>> Create([FromBody] CreateInformationCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Created($"api/information/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<InformationResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetInformationQuery(id));
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(InformationResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<InformationResponse>> Update([FromBody] UpdateInformationCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
