using Ecommerce_website.Helpers;
using Ecommerce_website.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptionService : IEncryptionService
{
    private readonly EncryptionConfiguration _config;

    public EncryptionService(IOptions<EncryptionConfiguration> config)
    {
        _config = config.Value;

        if (_config.Key == null || _config.Iv == null)
            throw new InvalidOperationException("Encryption key and IV must be configured.");
    }

    public string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = _config.Key;
            aes.IV = _config.Iv;

            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentException("Cipher text cannot be null or empty.", nameof(cipherText));

        using (Aes aes = Aes.Create())
        {
            aes.Key = _config.Key;
            aes.IV = _config.Iv;

            using var decryptor = aes.CreateDecryptor();
            var cipherBytes = Convert.FromBase64String(cipherText);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }

    public string Encrypt(int value)
    {
        return Encrypt(value.ToString());
    }

    public int DecryptToInt(string encryptedValue)
    {
        var decrypted = Decrypt(encryptedValue);
        return int.Parse(decrypted);
    }
}
