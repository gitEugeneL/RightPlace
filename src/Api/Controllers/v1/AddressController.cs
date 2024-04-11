using Api.Helpers;
using Application.Operations.Addresses;
using Application.Operations.Addresses.Commands.CreateAddress;
using Application.Operations.Addresses.Commands.UpdateAddress;
using Application.Operations.Addresses.Queries.GetAddress;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/addresses")]
public class AddressController : BaseController
{
    public AddressController(IMediator mediator) : base(mediator) { }
    
    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPost]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AddressResponse>> Create([FromBody] CreateAddressCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Created($"api/address/{result.Id}", result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddressResponse>> GetOne(Guid id)
    {
        var result = await Mediator.Send(new GetAddressQuery(id));
        return Ok(result);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPut]
    [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AddressResponse>> Update([FromBody] UpdateAddressCommand command)
    {
        command.SetCurrentUserId(CurrentUserId());
        var result = await Mediator.Send(command);
        return Ok(result);
    }
}
