using Application.Operations.Categories;
using Application.Operations.Categories.Queries.GetAllCategories;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/categories")]
public class CategoryController : BaseController
{
    public CategoryController(IMediator mediator) : base(mediator) { }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CategoryResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<CategoryResponse>>> GetAll()
    {
        var result = await Mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }
}