using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _dataContext;
    
    public ImageRepository(ApplicationDbContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Image> CreateImageAsync(Image image, CancellationToken cancellationToken)
    { 
        await _dataContext.Images
            .AddAsync(image, cancellationToken);
        await _dataContext.SaveChangesAsync(cancellationToken);
        return image;
    }

    public async Task<Image?> FindImageByBucketNameAndFileNameAsync(string bucketName, string fileName,
        CancellationToken cancellationToken)
    {
        return await _dataContext.Images
            .FirstOrDefaultAsync(image => 
                    image.FileName == fileName 
                    && image.BucketName == bucketName, 
                cancellationToken
            );
    }

    public async Task DeleteImageAsync(Image image, CancellationToken cancellationToken)
    {
        _dataContext.Images.Remove(image);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteImagesAsync(string bucketName, CancellationToken cancellationToken)
    {
        var images = await _dataContext.Images
            .Where(image => image.BucketName == bucketName)
            .ToListAsync(cancellationToken);
        
        _dataContext.Images.RemoveRange(images);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}
