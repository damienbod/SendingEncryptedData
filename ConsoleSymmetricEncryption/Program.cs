using EncryptDecryptLib;

namespace ConsoleCreateEncryptedText;

class Program
{
    static void Main(string[] args)
    {  
        var text = "I have a big dog. You've got a cat. We all love animals!";

        Console.WriteLine("-- Encrypt Decrypt symmetric --");
        Console.WriteLine("");

        var symmetricEncryptDecrypt = new SymmetricEncryptDecrypt();
        var (Key, IVBase64) = symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();

        var encryptedText = symmetricEncryptDecrypt.Encrypt(text, IVBase64, Key);

        Console.WriteLine("-- Key --");
        Console.WriteLine(Key);
        Console.WriteLine("-- IVBase64 --");
        Console.WriteLine(IVBase64);

        Console.WriteLine("");
        Console.WriteLine("-- Encrypted Text --");
        Console.WriteLine(encryptedText);

        var decryptedText = symmetricEncryptDecrypt.Decrypt(encryptedText, IVBase64, Key);

        Console.WriteLine("-- Decrypted Text --");
        Console.WriteLine(decryptedText);
    }
}