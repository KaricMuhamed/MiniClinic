using System;
using System.Security.Cryptography;

public class AuthService
{
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16); 
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32); 

        byte[] hashBytes = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);

        return Convert.ToBase64String(hashBytes); 
    }

    public static bool VerifyPassword(string password, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);
        byte[] salt = new byte[16];
        Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        for (int i = 0; i < hash.Length; i++)
        {
            if (hash[i] != hashBytes[salt.Length + i])
                return false;
        }
        return true;
    }
}
