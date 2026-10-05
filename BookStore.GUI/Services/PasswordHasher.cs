using System.Security.Cryptography;
using System.Text;

namespace BookStore.GUI.Services;

/// <summary>
/// API принимает уже готовый хеш пароля (CreateUserRequest.PasswordHash),
/// поэтому открытый пароль из формы на сервер не отправляется.
/// </summary>
public static class PasswordHasher
{
    public static string Hash(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        return Convert.ToHexStringLower(bytes);
    }
}
