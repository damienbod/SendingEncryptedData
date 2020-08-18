using CertificateManager;
using EncryptDecryptLib;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleAsymmetricEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var serviceProvider = new ServiceCollection()
                .AddCertificateManager()
                .BuildServiceProvider();

            var cc = serviceProvider.GetService<CreateCertificates>();

            var cert2048 = CreateRsaCertificates.CreateRsaCertificate(cc, 2048);

            var text = "I have a big dog. You've got a cat. We all love animals!";

            Console.WriteLine("-- Encrypt Decrypt asymmetric --");
            Console.WriteLine("");

            var asymmetricEncryptDecrypt = new AsymmetricEncryptDecrypt();

            var encryptedText = asymmetricEncryptDecrypt.Encrypt(text,
                Utils.CreateRsaPublicKey(cert2048));

            Console.WriteLine("");
            Console.WriteLine("-- Encrypted Text --");
            Console.WriteLine(encryptedText);

            var decryptedText = asymmetricEncryptDecrypt.Decrypt(encryptedText,
               Utils.CreateRsaPrivateKey(cert2048));

            Console.WriteLine("-- Decrypted Text --");
            Console.WriteLine(decryptedText);
        }
    }
}


