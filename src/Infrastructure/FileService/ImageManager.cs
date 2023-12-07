using System.Reactive.Linq;
using Application.Common.Interfaces;
using Minio;

namespace Infrastructure.FileService;

public sealed class ImageManager : IImageManager
{
    private readonly MinioClient _minioClient;
    
    public ImageManager(MinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<bool> BucketExists(string bucketName)
    {
        return await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName));
    }

    public async Task CreateBucket(string bucketName)
    {
        if (!await BucketExists(bucketName))
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
    }
    
    public async Task<int> CountFiles(string bucketName)
    {
        var objects = _minioClient.ListObjectsAsync(new ListObjectsArgs().WithBucket(bucketName));
        return await objects.Count();
    }
    
    public async Task Upload(string bucketName, string fileName, string fileType, long fileLength, Stream fileStream)
    {
        await CreateBucket(bucketName);
        
        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileLength)
                    .WithContentType(fileType)
        );
    }

    public async Task<MemoryStream> Download(string bucketName, string fileName)
    {
        var memoryStream = new MemoryStream();
        var tcs = new TaskCompletionSource<bool>();
        
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithCallbackStream(callbackStream =>
            {
                callbackStream.CopyTo(memoryStream);
                tcs.SetResult(true);
            });

        await _minioClient.GetObjectAsync(getObjectArgs);
        await tcs.Task;
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public async Task DeleteFile(string bucketName, string fileName)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName));    
    }

    public async Task DeleteBucket(string bucketName)
    {
        var objects = _minioClient.ListObjectsAsync(new ListObjectsArgs().WithBucket(bucketName));

        var count = await objects.Count();
        foreach (var obj in objects)
            await DeleteFile(bucketName, obj.Key);
        
        await _minioClient.RemoveBucketAsync(new RemoveBucketArgs().WithBucket(bucketName));
    }
}