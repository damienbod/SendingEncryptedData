using System;
using System.Security.Cryptography;
using System.Text;


namespace EncryptDecryptLib
{
    public class SymmetricEncryptDecrypt
    {

        public (string Key, string IVBase64) InitSession()
        {
            var key = GetEncodedRandomString(32); // 256
            Aes cipher = CreateCipher(key);
            var IVBase64 = Convert.ToBase64String(cipher.IV);
            return (key, IVBase64);
        }

        public string Encrypt(string text, string IVBase64, string cipherKeyBase64)
        {
            Aes cipher = CreateCipher(cipherKeyBase64);
            cipher.IV = Convert.FromBase64String(IVBase64);

            ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string cipherTextBase64, string IVBase64, string cipherKeyBase64)
        {
            Aes cipher = CreateCipher(cipherKeyBase64);
            cipher.IV = Convert.FromBase64String(IVBase64);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] cipherText = Convert.FromBase64String(cipherTextBase64);
            byte[] plainText = cryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);

            return Encoding.UTF8.GetString(plainText);
        }

        private string GetEncodedRandomString(int length)
        {
            var base64 = Convert.ToBase64String(GenerateRandomBytes(length));
            return base64;
        }

        private Aes CreateCipher(string keyBase64)
        {
            // Default values: Keysize 256, Mode CBC, Padding PKC27
            Aes cipher = Aes.Create();

            cipher.Padding = PaddingMode.ISO10126;
            cipher.Key = Convert.FromBase64String(keyBase64);

            return cipher;
        }

        private byte[] GenerateRandomBytes(int length)
        {
            using var randonNumberGen = new RNGCryptoServiceProvider();
            var byteArray = new byte[length];
            randonNumberGen.GetBytes(byteArray);
            return byteArray;
        }
    }
}
