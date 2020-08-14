using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace EncryptDecryptLib
{
    public class SymmetricEncryptDecrypt
    {
        private Aes CreateCipher(string keyBase64)
        {
            Aes cipher = Aes.Create();  //Defaults - Keysize 256, Mode CBC, Padding PKC27

            cipher.Padding = PaddingMode.ISO10126;

            //Create() makes new key each time, use a consistent key for encrypt/decrypt
            var stringd = WebEncoders.Base64UrlDecode(keyBase64);
            cipher.Key = stringd;

            return cipher;
        }

        /// <summary>
        ///  returns a base64 CipherText and a IV string to decrypt
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string CipherTextBase64, string IVBase64) Encrypt(string text, string cipherKeyBase64)
        {
            Aes cipher = CreateCipher(cipherKeyBase64);
            var IVBase64 = Convert.ToBase64String(cipher.IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return (Convert.ToBase64String(cipherText), IVBase64);
        }

        public string Decrypt(string cipherTextBase64, string cipherKeyBase64, string IVBase64)
        {
            Aes cipher = CreateCipher(cipherKeyBase64);
            cipher.IV = Convert.FromBase64String(IVBase64);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherText = Convert.FromBase64String(cipherTextBase64);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        public string GetEncodedRandomString()
        {
            var base64 = Convert.ToBase64String(GenerateRandomBytes(100));
            return base64;
        }

        public byte[] GenerateRandomBytes(int length)
        {
            using var randonNumberGen = new RNGCryptoServiceProvider();
            var byteArray = new byte[length];
            randonNumberGen.GetBytes(byteArray);
            return byteArray;
        }
    }
}
