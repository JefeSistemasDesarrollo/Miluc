using Miluc.Server.Interfaces.Encriptacion;
using System.Security.Cryptography;
using System.Text;

namespace Miluc.Server.Servicios
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(IConfiguration  configuration)
        {
           
            var key = configuration["Encryption:Key"];
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("No se encontro la llave en la configuración.");

            _key=Encoding.UTF8.GetBytes(key);

            if (_key.Length != 32)
                throw new InvalidOperationException("La clave de encriptación debe tener exactamente 32 caracteres.");
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cipherText))
                    return string.Empty;

                byte[] fullCipher = Convert.FromBase64String(cipherText);

                byte[] nonce = new byte[12];
                byte[] tag = new byte[16];
                byte[] cipherBytes = new byte[fullCipher.Length - nonce.Length - tag.Length];

                Buffer.BlockCopy(fullCipher, 0, nonce, 0, nonce.Length);
                Buffer.BlockCopy(fullCipher, nonce.Length, tag, 0, tag.Length);
                Buffer.BlockCopy(fullCipher, nonce.Length + tag.Length, cipherBytes, 0, cipherBytes.Length);

                byte[] plaintext = new byte[cipherBytes.Length];

                using var aes = new AesGcm(_key);

                aes.Decrypt(
                    nonce,
                    cipherBytes,
                    tag,
                    plaintext);

                return Encoding.UTF8.GetString(plaintext);
            }
            catch(Exception ex) 
            {

                throw new Exception(ex.Message);
            }
        }
    


public string Encrypt(string plainText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(plainText))
                    return string.Empty;

                byte[] nonce = RandomNumberGenerator.GetBytes(12);
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);

                byte[] cipherBytes = new byte[plaintextBytes.Length];
                byte[] tag = new byte[16];
                using var aes = new AesGcm(_key);

                aes.Encrypt(
                    nonce,
                    plaintextBytes,
                    cipherBytes,
                    tag);

                byte[] result = new byte[
                    nonce.Length +
                    tag.Length +
                    cipherBytes.Length];

                Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
                Buffer.BlockCopy(tag, 0, result, nonce.Length, tag.Length);
                Buffer.BlockCopy(cipherBytes, 0, result, nonce.Length + tag.Length, cipherBytes.Length);

                return Convert.ToBase64String(result);

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            
            }
        }
    }
}
