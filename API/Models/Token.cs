namespace API.Models;

public record Token
{
    public required string Type { get; set; }
    public required string AccessToken { get; set; }
}