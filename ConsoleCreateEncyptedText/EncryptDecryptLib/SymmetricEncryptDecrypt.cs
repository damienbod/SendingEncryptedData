using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecryptLib
{
    public class SymmetricEncryptDecrypt
    {
        private Aes CreateCipher(byte[] key)
        {
            Aes cipher = Aes.Create();  //Defaults - Keysize 256, Mode CBC, Padding PKC27

            cipher.Padding = PaddingMode.ISO10126;

            //Create() makes new key each time, use a consistent key for encrypt/decrypt
            cipher.Key = key;

            return cipher;
        }

        /// <summary>
        ///  returns a base64 CipherText and a IV string to decrypt
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public (string CipherTextBase64, string IV) Encrypt(string text, byte[] cipherkey)
        {
            Aes cipher = CreateCipher(cipherkey);
            var IV = Convert.ToBase64String(cipher.IV);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return (Convert.ToBase64String(cipherText), IV);
        }

        public string Decrypt(string cipherTextBase64, byte[] cipherkey, string base64IV)
        {
            Aes cipher = CreateCipher(cipherkey);
            cipher.IV = Convert.FromBase64String(base64IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherText = Convert.FromBase64String(cipherTextBase64);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainText);
        }
    }
}
