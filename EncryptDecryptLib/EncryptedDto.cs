namespace EncryptDecryptLib
{
    public class EncryptedDto
    {
        public string EncryptedText { get; set; }
        public string IV { get; set; }
        public string Key { get; set; }
        public string DigitalSignature { get; set; }
        public string Sender { get; set; }
    }
}
