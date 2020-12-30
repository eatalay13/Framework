using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Helpers
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
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




        #region NewEncrypt
        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="plainText">String to be encrypted</param>
        /// <param name="password">Password</param>
        public string Encrypt(string plainText)
        {
            if (plainText == null)
            {
                return null;
            }

            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(EncryptionKey);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Decrypt a string.
        /// </summary>
        /// <param name="encryptedText">String to be decrypted</param>
        /// <param name="password">Password used during encryption</param>
        /// <exception cref="FormatException"></exception>
        public string Decrypt(string encryptedText)
        {
            if (encryptedText == null)
            {
                return null;
            }

            // Get the bytes of the string
            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(EncryptionKey);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        private byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        #endregion



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