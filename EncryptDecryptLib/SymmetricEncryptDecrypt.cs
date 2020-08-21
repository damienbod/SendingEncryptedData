using System;
using System.Security.Cryptography;
using System.Text;


namespace EncryptDecryptLib
{
    public class SymmetricEncryptDecrypt
    {

        public (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
        {
            var key = GetEncodedRandomString(32); // 256
            Aes cipher = CreateCipher(key);
            var IVBase64 = Convert.ToBase64String(cipher.IV);
            return (key, IVBase64);
        }

        /// <summary>
        /// Encrypt using AES
        /// </summary>
        /// <param name="text">any text</param>
        /// <param name="IV">Base64 IV string/param>
        /// <param name="key">Base64 key</param>
        /// <returns>Returns an encrypted string</returns>
        public string Encrypt(string text, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Decrypt using AES
        /// </summary>
        /// <param name="text">Base64 string for an AES encryption</param>
        /// <param name="IV">Base64 IV string/param>
        /// <param name="key">Base64 key</param>
        /// <returns>Returns a string</returns>
        public string Decrypt(string encryptedText, string IV, string key)
        {
            Aes cipher = CreateCipher(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

        private string GetEncodedRandomString(int length)
        {
            var base64 = Convert.ToBase64String(GenerateRandomBytes(length));
            return base64;
        }

        /// <summary>
        /// Create an AES Cipher using a base64 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>AES</returns>
        private Aes CreateCipher(string keyBase64)
        {
            // Default values: Keysize 256, Padding PKC27
            Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC; // Ensuring the integrity of the ciphertext if using CBC
            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }

        private byte[] GenerateRandomBytes(int length)
        {
            var byteArray = new byte[length];
            RandomNumberGenerator.Fill(byteArray);
            return byteArray;
        }
    }
}
