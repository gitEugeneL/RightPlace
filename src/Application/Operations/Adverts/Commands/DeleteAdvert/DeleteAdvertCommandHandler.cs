using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Adverts.Commands.DeleteAdvert;

public class DeleteAdvertCommandHandler : IRequestHandler<DeleteAdvertCommand, Unit>
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageManager _imageManager;
    
    public DeleteAdvertCommandHandler(
        IAdvertRepository advertRepository, IImageRepository imageRepository, IImageManager imageManagers)
    {
        _advertRepository = advertRepository;
        _imageRepository = imageRepository;
        _imageManager = imageManagers;
    }

    public async Task<Unit> Handle(DeleteAdvertCommand request, CancellationToken cancellationToken)
    {
        var advertisement =
            await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
            ?? throw new NotFoundException(nameof(Advert), request.AdvertId);
        
        if (advertisement.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);

        // delete images
        await _imageRepository.DeleteImagesAsync(request.AdvertId.ToString(), cancellationToken);
        await _imageManager.DeleteBucket(request.AdvertId.ToString());
        
        await _advertRepository.DeleteAdvertAsync(advertisement, cancellationToken);
        return await Unit.Task;
    }
}
