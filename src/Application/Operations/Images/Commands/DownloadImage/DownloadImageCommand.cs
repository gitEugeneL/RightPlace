using MediatR;

namespace Application.Operations.Images.Commands.DownloadImage;

public record DownloadImageCommand(
    Guid AdvertId, 
    string ImageName
) : IRequest<ImageResponse>;
