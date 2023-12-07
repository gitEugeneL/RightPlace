using MediatR;

namespace Application.Operations.Images.Commands.UploadImage;

public record UploadImageCommand : IRequest<Unit>
{
    public required Guid CurrentUserId { get; init; }
    public required Guid AdvertId { get; init; }
    public required string FileType { get; init; }
    public required long FileLength { get; init;  }
    public required Stream FileStream { get; init; }
}
