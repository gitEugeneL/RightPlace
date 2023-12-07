namespace Application.Operations.Images;

public record ImageResponse(
    MemoryStream MemoryStream,
    string ContentType,
    string FileName
);