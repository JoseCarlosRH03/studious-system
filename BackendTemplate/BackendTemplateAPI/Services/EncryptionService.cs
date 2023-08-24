using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BackendTemplateCore.Services;

namespace BackendTemplateAPI.Services;

public class EncryptionService : IEncryptionService {
   public EncryptionService(IConfiguration config) {
      EncryptionKey = config.GetValue<string>(nameof(EncryptionKey));
      if (EncryptionKey is null)
         throw new EncryptionServiceException("API's encryption key not configured.");
   }

   readonly string EncryptionKey;

   public class EncryptionServiceException : Exception {
      public EncryptionServiceException(string? message) : base(message) { }
   }

   public string Encrypt(string data) {
      using var aes = Aes.Create();
      aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
      aes.IV = new byte[16];

      ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

      var memoryStream = new MemoryStream();
      using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
         using var streamWriter = new StreamWriter(cryptoStream);
         streamWriter.Write(data);
      }

      return Convert.ToBase64String(memoryStream.ToArray());
   }

   public string Encrypt<T>(T data) =>
      Encrypt(JsonSerializer.Serialize(data));

   public string Decrypt(string cypher) {
      byte[] buffer = Convert.FromBase64String(cypher.Replace(' ', '+'));

      using var aes = Aes.Create();
      aes.Key = Encoding.UTF8.GetBytes(EncryptionKey);
      aes.IV = new byte[16];
      ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

      using var memoryStream = new MemoryStream(buffer);
      using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
      using var streamReader = new StreamReader(cryptoStream);

      return streamReader.ReadToEnd();
   }

   public T Decrypt<T>(string cypher) =>
      JsonSerializer.Deserialize<T>(Decrypt(cypher)) ?? throw new ArgumentException("Cifrado invalido");
}