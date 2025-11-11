using Aviscom.Services.Interfaces;
using System.Security.Cryptography;

namespace Aviscom.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key;

        public EncryptionService(IConfiguration configuration)
        {
            // === CORREÇÃO AQUI ===
            // Altere de "Encryption:Key" para "EncryptionSettings:AESKey"
            var keyBase64 = configuration["EncryptionSettings:AESKey"];

            if (string.IsNullOrEmpty(keyBase64))
            {
                // (Aqui já estava correto, mantemos)
                throw new ArgumentException("Chave AES (EncryptionSettings:AESKey) não encontrada no appsettings.");
            }

            _key = Convert.FromBase64String(keyBase64);
        }

        // ... O restante do arquivo (Encrypt/Decrypt) está correto ...

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;

            // Gera um IV (Vetor de Inicialização) aleatório e único
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var msEncrypt = new MemoryStream();

            // Escreve o IV no início do stream
            msEncrypt.Write(iv, 0, iv.Length);

            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            // Retorna o [IV + TextoCriptografado] em Base64
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = _key;

            // Extrai o IV do início do array de bytes
            var iv = new byte[aes.BlockSize / 8];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);

            // O texto criptografado real começa *após* o IV
            var encryptedBytes = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            using var msDecrypt = new MemoryStream(encryptedBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}