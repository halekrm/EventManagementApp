namespace Services.Contracts
{
    public interface IEncryptionService
    {
        string Encrypt(string text);
        string Decrypt(string encryptedText);
    }
}