using Application.Operations.Types;
using Application.Operations.Types.Queries.GetAllTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/types")]
public class TypeController : BaseController
{
    public TypeController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<TypeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<TypeResponse>>> GetAll()
    {
        var result = await Mediator.Send(new GetAllTypesQuery());
        return Ok(result);
    }
    
    // todo add (admin)
    // todo delete (admin)
    // todo update (admin)
}