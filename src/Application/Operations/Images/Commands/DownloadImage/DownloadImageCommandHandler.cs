using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Operations.Images.Commands.DownloadImage;

public class DownloadImageCommandHandler : IRequestHandler<DownloadImageCommand, ImageResponse>
{
    private readonly IAdvertRepository _advertRepository;
    private readonly IImageRepository _imageRepository;
    private readonly IImageManager _imageManager;
    
    public DownloadImageCommandHandler(
        IImageManager imageManager, IAdvertRepository advertRepository, IImageRepository imageRepository)
    {
        _imageManager = imageManager;
        _advertRepository = advertRepository;
        _imageRepository = imageRepository;
    }
    
    public async Task<ImageResponse> Handle(DownloadImageCommand request, CancellationToken cancellationToken)
    {
        var advert = await _advertRepository.FindAdvertByIdAsync(request.AdvertId, cancellationToken)
                     ?? throw new NotFoundException(nameof(Advert), request.AdvertId);

        if (advert.Images.All(image => image.FileName != request.ImageName))
            throw new NotFoundException(nameof(Advert), request.ImageName);
        
        var bucketName = request.AdvertId.ToString();
        var memoryStream = await _imageManager.Download(bucketName, request.ImageName);

        return new ImageResponse(memoryStream, "image/png", request.ImageName);
    }
}
