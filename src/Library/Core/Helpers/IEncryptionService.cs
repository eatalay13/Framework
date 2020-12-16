using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers
{
    public interface IEncryptionService
    {
        string CreateSaltKey(int size);
        string CreatePasswordHash(string password, string saltKey, string passwordFormat = "SHA1");
        string EncryptText(string plainText, string encryptionPrivateKey = "");
        string DecryptText(string cipherText, string encryptionPrivateKey = "", bool isUrl = false);
        string EncryptByHex(string textValue);
        string DecryptByHex(string hexValue);
        string EncryptPassword(string password);
        string DecryptPassword(string decryptedPassword, bool isUrl = false);
    }

    public class EncryptionService : IEncryptionService
    {
        private const string EncryptionKey = "565ydk8f98dc431f";
        private const string PasswordEncryptionKey = "565dmk8f98ar431f";

        public virtual string CreateSaltKey(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public virtual string CreatePasswordHash(string password, string saltKey, string passwordFormat = "SHA1")
        {
            if (string.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";

            var saltAndPassword = string.Concat(password, saltKey);

            var algorithm = HashAlgorithm.Create(passwordFormat);
            if (algorithm == null)
                throw new ArgumentException("Unrecognized hash name", "passwordFormat");

            var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = EncryptionKey;

            var tDesAlg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var encryptedBinary = EncryptTextToMemory(plainText, tDesAlg.Key, tDesAlg.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "", bool isUrl = false)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = EncryptionKey;

            if (isUrl)
                cipherText = cipherText.Replace(" ", "+");

            var tDesAlg = new TripleDESCryptoServiceProvider
            {
                Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var buffer = Convert.FromBase64String(cipherText);
            return DecryptTextFromMemory(buffer, tDesAlg.Key, tDesAlg.IV);
        }

        public string EncryptByHex(string textValue)
        {
            var stringBytes = Encoding.Unicode.GetBytes(textValue);
            var sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (var b in stringBytes) sbBytes.AppendFormat("{0:X2}", b);
            return sbBytes.ToString();
        }

        public string DecryptByHex(string hexValue)
        {
            var numberChars = hexValue.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2) bytes[i / 2] = Convert.ToByte(hexValue.Substring(i, 2), 16);
            return Encoding.Unicode.GetString(bytes);
        }

        public string EncryptPassword(string password)
        {
            return EncryptText(password, PasswordEncryptionKey);
        }

        public string DecryptPassword(string decryptedPassword, bool isUrl = false)
        {
            return DecryptText(decryptedPassword, PasswordEncryptionKey, isUrl);
        }

        #region Utilities

        private static byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
                CryptoStreamMode.Write);
            var toEncrypt = new UnicodeEncoding().GetBytes(data);
            cs.Write(toEncrypt, 0, toEncrypt.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        private static string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream(data);
            using var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
                CryptoStreamMode.Read);
            var sr = new StreamReader(cs, new UnicodeEncoding());
            return sr.ReadLine();
        }

        #endregion
    }
}