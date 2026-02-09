using Microsoft.AspNetCore.Identity;

namespace Fly_Away.Services;

public class PasswordService
{
    private readonly PasswordHasher<string> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword("user", password);

    public bool Verify(string hashed, string password)
        => _hasher.VerifyHashedPassword("user", hashed, password) == PasswordVerificationResult.Success;
}
