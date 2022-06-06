using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using CertificateManager;
using EncryptDecryptLib;
using ExchangeSecureTexts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ExchangeSecureTexts.Pages
{
    public class DecryptTextModel : PageModel
    {
        private readonly SymmetricEncryptDecrypt _symmetricEncryptDecrypt;
        private readonly AsymmetricEncryptDecrypt _asymmetricEncryptDecrypt;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ImportExportCertificate _importExportCertificate;
        private readonly DigitalSignatures _digitalSignatures;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string? Message { get; set; }

        [BindProperty]
        [Required]
        public string EncryptedMessage { get; set; } = string.Empty;

        public DecryptTextModel(SymmetricEncryptDecrypt symmetricEncryptDecrypt,
            AsymmetricEncryptDecrypt asymmetricEncryptDecrypt,
            ApplicationDbContext applicationDbContext,
            ImportExportCertificate importExportCertificate,
            DigitalSignatures digitalSignatures,
            IConfiguration configuration)
        {
            _symmetricEncryptDecrypt = symmetricEncryptDecrypt;
            _asymmetricEncryptDecrypt = asymmetricEncryptDecrypt;
            _applicationDbContext = applicationDbContext;
            _importExportCertificate = importExportCertificate;
            _digitalSignatures = digitalSignatures;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Something failed. Redisplay the form.
                return OnGet();
            }

            var cert = GetCertificateWithPrivateKeyForIdentity();

            var encryptedDto = JsonSerializer.Deserialize<EncryptedDto>(EncryptedMessage);

            var sender = _asymmetricEncryptDecrypt.Decrypt(encryptedDto.Sender,
               Utils.CreateRsaPrivateKey(cert));

            var senderCert = GetCertificateWithPublicKeyForIdentity(sender);

            var verified = _digitalSignatures.Verify(encryptedDto.EncryptedText, 
                encryptedDto.DigitalSignature,
                Utils.CreateRsaPublicKey(senderCert));

            if(!verified) return BadRequest("NOT verified");

            var key = _asymmetricEncryptDecrypt.Decrypt(encryptedDto.Key,
               Utils.CreateRsaPrivateKey(cert));

            var IV = _asymmetricEncryptDecrypt.Decrypt(encryptedDto.IV,
               Utils.CreateRsaPrivateKey(cert));

            var text = _symmetricEncryptDecrypt.Decrypt(encryptedDto.EncryptedText, IV, key);

            Message = $"{text}";

            // Redisplay the form.
            return OnGet();

        }

        private X509Certificate2 GetCertificateWithPrivateKeyForIdentity()
        {
            var user = _applicationDbContext.Users.First(user => user.Email == User.Identity.Name);

            var cert = _importExportCertificate.PemImportCertificate(user.PemPrivateKey,
                _configuration["PemPasswordExportImport"]);
            return cert;
        }

        private X509Certificate2 GetCertificateWithPublicKeyForIdentity(string email)
        {
            var user = _applicationDbContext.Users.First(user => user.Email == email);
            var cert = _importExportCertificate.PemImportCertificate(user.PemPublicKey);
            return cert;
        }
    }
}
