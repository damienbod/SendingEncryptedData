using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace EncryptDecryptLib;

public static class Utils
{
    public static RSA CreateRsaPublicKey(X509Certificate2 certificate)
    {
        var publicKeyProvider = certificate.GetRSAPublicKey();

        if (publicKeyProvider == null)
            throw new ArgumentException("RSA public key is null");

        return publicKeyProvider;
    }

    public static RSA CreateRsaPrivateKey(X509Certificate2 certificate)
    {
        var privateKeyProvider = certificate.GetRSAPrivateKey();

        if (privateKeyProvider == null)
            throw new ArgumentException("RSA private key is null");

        return privateKeyProvider;
    }
}
