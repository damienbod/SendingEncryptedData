using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EncryptDecryptLib
{
    public class EncryptText
    {
        private X509Certificate2 ReceiverPublicKeyCertificate;
        private X509Certificate2 PrivateKeyCertificate;

        public EncryptText()
        {

        }

        public string Encrypt(string text)
        {
            return null;
        }

        private byte[] EncryptRsa(byte[] data, X509Certificate2 rsaCertificate)
        {
            return null;
            //RSAParameters rSAParameters = rsaCertificate.;
            //using (var rsa = new RSACryptoServiceProvider())
            //{
            //    rsa.ImportParameters(rSAParameters);
            //    return rsa.Encrypt(data, false);

            //}
        }
    }
}
