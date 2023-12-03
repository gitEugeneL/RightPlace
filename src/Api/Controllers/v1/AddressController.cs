using Application.Operations.Addresses;
using Application.Operations.Addresses.Commands.CreateAddress;
using Application.Operations.Addresses.Commands.UpdateAddress;
using Application.Operations.Addresses.Queries.GetAddress;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/addreses")]
public class AddressController : BaseController
{
    public AddressController(IMediator mediator) : base(mediator) { }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AddressResponse>> Create([FromBody] CreateAddressCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();

        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Created($"api/address/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetAddressQuery(id));
        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AddressResponse>> Update([FromBody] UpdateAddressCommand command)
    {
        var id = CurrentUserId();
        if (id is null)
            return BadRequest();
        
        command.SetCurrentUserId(id);
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
