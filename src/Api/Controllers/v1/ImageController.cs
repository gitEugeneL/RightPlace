using Api.Helpers;
using Application.Operations.Images.Commands.DeleteImage;
using Application.Operations.Images.Commands.DownloadImage;
using Application.Operations.Images.Commands.UploadImage;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[ApiVersion(1)]
[Route("api/v{v:apiVersion}/files")]
public class ImageController : BaseController
{
    public ImageController(IMediator mediator) : base(mediator) { }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpPost("upload-image/{advertId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> UploadImage(IFormFile file, Guid advertId)
    {
        /*
         * [!]
         * You should add a file signature validator to your production API
         * https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-7.0#file-signature-validation
         * [!]
         */
        if (Path.GetExtension(file.FileName) != ".png")
            return BadRequest();

        var command = new UploadImageCommand
        {
            CurrentUserId = Guid.Parse(CurrentUserId()),
            AdvertId = advertId,
            FileType = "image/png",
            FileLength = file.Length,
            FileStream = file.OpenReadStream()
        };
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpGet("download-image/{advertId:guid}/{imageName}")]
    [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DownloadImage(Guid advertId, string imageName)
    {
        var result = await Mediator.Send(new DownloadImageCommand(advertId, imageName));
        return File(result.MemoryStream, result.ContentType, result.FileName);
    }

    [Authorize(AppConstants.BaseAuthPolicy)]
    [HttpDelete("delete-image/{advertId:guid}/{imageName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteImage(Guid advertId, string imageName)
    {
        await Mediator.Send(new DeleteImageCommand(
                Guid.Parse(CurrentUserId()), 
                advertId, 
                imageName)
        );
        return Ok();
    }
}
