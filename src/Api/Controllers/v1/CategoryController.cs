using Application.Categories;
using Application.Categories.Queries.GetAllCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/categories")]
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
    
    // todo add (admin)
    // todo delete (admin)
    // todo update (admin)
}