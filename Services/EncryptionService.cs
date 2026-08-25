using System.Security.Cryptography;
using System.Text;
using Services.Contracts;

namespace Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(string key)
        {
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        }
        public string Encrypt(string text)
        {
            using Aes aes = Aes.Create();

            aes.Key = _key;
            aes.GenerateIV();

            using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
            byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];

            Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);

            Array.Copy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);

            return Convert.ToBase64String(result);
        }
        public string Decrypt(string encryptedText)
        {
            byte[] fullData = Convert.FromBase64String(encryptedText);

            using Aes aes = Aes.Create();

            aes.Key = _key;

            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] encryptedBytes = new byte[fullData.Length - iv.Length];

            Array.Copy(fullData, 0, iv, 0, iv.Length);

            Array.Copy(fullData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            aes.IV = iv;

            using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }


    }
}