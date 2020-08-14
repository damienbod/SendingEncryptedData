using EncryptDecryptLib;
using System;
using System.Text.Encodings.Web;

namespace ConsoleCreateEncryptedText
{
    class Program
    {
        static void Main(string[] args)
        {
            var text = "I have a big dog.";

            Console.WriteLine("-- Encrypt Decrypt symmetric --");
            Console.WriteLine("");

            var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
            var key = symmetricEncryptDecrypt.GetEncodedRandomString();
            var result = symmetricEncryptDecrypt.Encrypt(text, key);

            Console.WriteLine("-- Cipher Key --");
            Console.WriteLine(key);

            Console.WriteLine("-- CipherTextBase64 --");
            Console.WriteLine(result.CipherTextBase64);
            Console.WriteLine("");
            Console.WriteLine("-- IVBase64 --");
            Console.WriteLine(result.IVBase64);

            var decryptedText = symmetricEncryptDecrypt.Decrypt(result.CipherTextBase64, key, result.IVBase64);

            Console.WriteLine("-- Decrypted Text --");
            Console.WriteLine(decryptedText);
        }
    }
}
