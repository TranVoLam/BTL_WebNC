using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace BTL_WebNC.Utilities;

public static class PasswordHelper {
    private static readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
    public static string Hash(string password) {
        var hasherPassword = _passwordHasher.HashPassword(new object(), password);
        return hasherPassword;
    }

    public static bool Verify(string hasherPassword, string password) {
        var result = _passwordHasher.VerifyHashedPassword(new object(), hasherPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}