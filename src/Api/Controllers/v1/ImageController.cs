using Application.Operations.Images.Commands.DeleteImage;
using Application.Operations.Images.Commands.DownloadImage;
using Application.Operations.Images.Commands.UploadImage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1;

[Route("api/files")]
public class ImageController : BaseController
{
    public ImageController(IMediator mediator) : base(mediator) { }

    [Authorize]
    [HttpPost]
    [Route("Upload-image/{advertId:guid}")]
    public async Task<ActionResult> UploadImage(IFormFile file, Guid advertId)
    {
        var userId = CurrentUserId();
        if (userId is null)
            return BadRequest();
        
        if (Path.GetExtension(file.FileName) != ".png")
            return BadRequest();

        var command = new UploadImageCommand
        {
            CurrentUserId = Guid.Parse(userId),
            AdvertId = advertId,
            FileType = "image/png",
            FileLength = file.Length,
            FileStream = file.OpenReadStream()
        };
        await Mediator.Send(command);
        return Ok();
    }
    
    [HttpGet]
    [Route("download-image/{advertId:guid}/{imageName}")]
    public async Task<ActionResult> DownloadImage(Guid advertId, string imageName)
    {
        var result = await Mediator.Send(new DownloadImageCommand(advertId, imageName));
        return File(result.MemoryStream, result.ContentType, result.FileName);
    }

    [Authorize]
    [HttpDelete]
    [Route("delete-image/{advertId:guid}/{imageName}")]
    public async Task<ActionResult> DeleteImage(Guid advertId, string imageName)
    {
        var userId = CurrentUserId();
        if (userId is null)
            return BadRequest();
        
        await Mediator.Send(new DeleteImageCommand(Guid.Parse(userId), advertId, imageName));
        return Ok();
    }
}
