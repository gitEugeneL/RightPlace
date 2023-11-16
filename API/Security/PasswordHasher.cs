namespace API.Security;

public interface IPasswordHasher
{
    void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
    bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
} 

public class PasswordHasher : IPasswordHasher
{
    public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        salt = hmac.Key;
    }

    public bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}