using MediatR;

namespace Application.Operations.Images.Commands.DeleteImage;

public record DeleteImageCommand(
    Guid CurrentUserId,
    Guid AdvertId,
    string ImageName
) : IRequest<Unit>;