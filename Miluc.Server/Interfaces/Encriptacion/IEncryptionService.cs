namespace Miluc.Server.Interfaces.Encriptacion
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
