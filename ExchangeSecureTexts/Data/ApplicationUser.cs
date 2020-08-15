using Microsoft.AspNetCore.Identity;

namespace ExchangeSecureTexts.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string PemPrivateKey { get; set; }

        public string PemPublicKey { get; set; }
    }
}