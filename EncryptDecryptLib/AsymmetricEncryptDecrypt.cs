using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EncryptDecryptLib
{
    public class AsymmetricEncryptDecrypt
    {
        public string Encrypt(string text, RSA rsa)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string text, RSA rsa)
        {
            byte[] data = Convert.FromBase64String(text); 
            byte[] cipherText = rsa.Decrypt(data, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(cipherText);
        }
    }
}
