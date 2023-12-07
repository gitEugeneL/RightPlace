namespace Application.Common.Interfaces;

public interface IImageManager
{
    Task CreateBucket(string bucketName);
    
    Task<bool> BucketExists(string bucketName);
    
    Task<int> CountFiles(string bucketName);
    
    Task Upload(string bucketName, string fileName, string fileType, long fileLength, Stream fileStream);

    Task<MemoryStream> Download(string bucketName, string fileName);

    Task DeleteFile(string bucketName, string fileName);
  
    Task DeleteBucket(string bucketName);
}
