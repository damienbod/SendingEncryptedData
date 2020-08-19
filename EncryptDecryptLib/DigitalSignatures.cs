using System;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecryptLib
{
    public class DigitalSignatures
    {
        // Sign with RSA using private key
        public string Sign(string text, RSA rsa)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return Convert.ToBase64String(signature);
        }

        // Verify with RSA using public key
        public bool Verify(string text, string signatureBase64, RSA rsa)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            byte[] signature = Convert.FromBase64String(signatureBase64);
            bool isValid = rsa.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return isValid;
        }
    }
}
