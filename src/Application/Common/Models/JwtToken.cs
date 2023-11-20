namespace Application.Common.Models;

public record JwtToken(string Token, string Type = "Bearer");