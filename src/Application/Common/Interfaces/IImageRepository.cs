using Domain.Entities;

namespace Application.Common.Interfaces;

public interface IImageRepository
{
    Task<Image> CreateImageAsync(Image image, CancellationToken cancellationToken);

    Task<Image?> FindImageByBucketNameAndFileNameAsync(string bucketName, string fileName,
        CancellationToken cancellationToken);

    Task DeleteImageAsync(Image image, CancellationToken cancellationToken);

    Task DeleteImagesAsync(string bucketName, CancellationToken cancellationToken);
}