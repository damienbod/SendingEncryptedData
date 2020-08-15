using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EncryptDecryptLib
{
    public class AsymmetricEncryptDecrypt
    {
        public RSA CreateCipherPublicKey(X509Certificate2 certificate)
        {
            RSA publicKeyProvider = certificate.GetRSAPublicKey();
            return publicKeyProvider;
        }

        public RSA CreateCipherPrivateKey(X509Certificate2 certificate)
        {
            RSA privateKeyProvider = certificate.GetRSAPrivateKey();
            return privateKeyProvider;
        }

        public string Encrypt(string text, RSA cipher)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cipher.Encrypt(data, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string text, RSA cipher)
        {
            byte[] data = Convert.FromBase64String(text); 
            byte[] cipherText = cipher.Decrypt(data, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(cipherText);
        }
    }
}
