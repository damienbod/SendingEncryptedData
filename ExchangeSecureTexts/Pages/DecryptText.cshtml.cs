using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using CertificateManager;
using EncryptDecryptLib;
using ExchangeSecureTexts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExchangeSecureTexts.Pages
{
    public class DecryptTextModel : PageModel
    {
        private readonly SymmetricEncryptDecrypt _symmetricEncryptDecrypt;
        private readonly AsymmetricEncryptDecrypt _asymmetricEncryptDecrypt;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ImportExportCertificate _importExportCertificate;

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        [Required]
        public string EncryptedMessage { get; set; }

        public DecryptTextModel(SymmetricEncryptDecrypt symmetricEncryptDecrypt,
            AsymmetricEncryptDecrypt asymmetricEncryptDecrypt,
            ApplicationDbContext applicationDbContext,
            ImportExportCertificate importExportCertificate)
        {
            _symmetricEncryptDecrypt = symmetricEncryptDecrypt;
            _asymmetricEncryptDecrypt = asymmetricEncryptDecrypt;
            _applicationDbContext = applicationDbContext;
            _importExportCertificate = importExportCertificate;
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

            var key = _asymmetricEncryptDecrypt.Decrypt(encryptedDto.Key,
                _asymmetricEncryptDecrypt.CreateCipherPrivateKey(cert));

            var IV = _asymmetricEncryptDecrypt.Decrypt(encryptedDto.IV,
              _asymmetricEncryptDecrypt.CreateCipherPrivateKey(cert));

            var text = _symmetricEncryptDecrypt.Decrypt(encryptedDto.EncryptedText, IV, key);

            Message = $"{text}";

            // Redisplay the form.
            return OnGet();

        }

        private X509Certificate2 GetCertificateWithPrivateKeyForIdentity()
        {
            var user = _applicationDbContext.Users.First(user => user.Email == User.Identity.Name);

            var certWithPublicKey = _importExportCertificate.PemImportCertificate(user.PemPublicKey);
            var privateKey = _importExportCertificate.PemImportPrivateKey(user.PemPrivateKey);

            var cert = _importExportCertificate.CreateCertificateWithPrivateKey(
                certWithPublicKey, privateKey);

            return cert;
        }
    }
}
