using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Images.Commands.DeleteImage;

public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand, Unit>
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageManager _imageManager;
    
    public DeleteImageCommandHandler(
        IImageManager imageManager, IAdvertRepository advertRepository, IImageRepository imageRepository)
    {
        _imageManager = imageManager;
        _advertRepository = advertRepository;
        _imageRepository = imageRepository;
    }
    
    public async Task<Unit> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
    {
        var advert = await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        var image = await _imageRepository
                            .FindImageByBucketNameAndFileNameAsync(
                                request.AdvertId.ToString(),
                                request.ImageName,
                                cancellationToken)
                        ?? throw new NotFoundException(nameof(Images), request.ImageName);

        // minIO delete file
        await _imageManager.DeleteFile(request.AdvertId.ToString(), request.ImageName);

        // database delete info
        await _imageRepository.DeleteImageAsync(image, cancellationToken);
        return await Unit.Task;
    }
}