using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Images.Commands.UploadImage;

public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, Unit>
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageManager _imageManager;
    
    public UploadImageCommandHandler(
        IImageManager imageManager, IAdvertRepository advertRepository, IImageRepository imageRepository)
    {
        _imageManager = imageManager;
        _advertRepository = advertRepository;
        _imageRepository = imageRepository;
    }
    
    public async Task<Unit> Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        var advert = await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advert.UserId != request.CurrentUserId)
            throw new AccessDeniedException(nameof(Advert), request.AdvertId);
        
        var bucketName = advert.Id.ToString();
        await _imageManager.CreateBucket(bucketName);

        // each advert can only have 5 images
        if (await _imageManager.CountFiles(bucketName) >= 5)
            throw new AlreadyExistException(nameof(Image), "5 or more images already added");

        var fileName = $"{advert.Title}-{Guid.NewGuid()}.png";
        
        // save image in miniIO
        await _imageManager.Upload(bucketName, fileName, request.FileType, request.FileLength, request.FileStream);
        
        // save info in database
        await _imageRepository.CreateImageAsync(
            new Image
            {
                BucketName = bucketName,
                FileName = fileName,
                FileType = request.FileType,
                Advert = advert
            },
            cancellationToken
        );
        
        return await Unit.Task;
    }
}