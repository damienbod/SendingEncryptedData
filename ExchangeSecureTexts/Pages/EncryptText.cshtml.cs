using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using CertificateManager;
using EncryptDecryptLib;
using ExchangeSecureTexts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExchangeSecureTexts.Pages
{
    public class EncryptTextModel : PageModel
    {
        private readonly SymmetricEncryptDecrypt _symmetricEncryptDecrypt;
        private readonly AsymmetricEncryptDecrypt _asymmetricEncryptDecrypt;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ImportExportCertificate _importExportCertificate;
        private readonly DigitalSignatures _digitalSignatures;
        private readonly IConfiguration _configuration;

        [BindProperty]
        [Required]
        public string TargetUserEmail { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string Message { get; set; } = string.Empty;

        [BindProperty]
        public string? EncryptedMessage { get; set; }

        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();

        public EncryptTextModel(SymmetricEncryptDecrypt symmetricEncryptDecrypt,
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
            // not good if you have a lot of users
            Users = _applicationDbContext.Users.Select(a =>
                                 new SelectListItem
                                 {
                                     Value = a.Email.ToString(),
                                     Text = a.Email
                                 }).ToList();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Something failed. Redisplay the form.
                return OnGet();
            }

            var (Key, IVBase64) = _symmetricEncryptDecrypt.InitSymmetricEncryptionKeyIV();

            var encryptedText = _symmetricEncryptDecrypt.Encrypt(Message, IVBase64, Key);

            var targetUserPublicCertificate = GetCertificateWithPublicKeyForIdentity(TargetUserEmail);

            var encryptedKey = _asymmetricEncryptDecrypt.Encrypt(Key,
                Utils.CreateRsaPublicKey(targetUserPublicCertificate));

            var encryptedIV = _asymmetricEncryptDecrypt.Encrypt(IVBase64,
                Utils.CreateRsaPublicKey(targetUserPublicCertificate));

            var encryptedSender = _asymmetricEncryptDecrypt.Encrypt(User.Identity.Name,
                Utils.CreateRsaPublicKey(targetUserPublicCertificate));

            var certLoggedInUser = GetCertificateWithPrivateKeyForIdentity();

            var signature = _digitalSignatures.Sign(encryptedText,
                Utils.CreateRsaPrivateKey(certLoggedInUser));

            var encryptedDto = new EncryptedDto
            {
                EncryptedText = encryptedText,
                Key = encryptedKey,
                IV = encryptedIV,
                DigitalSignature = signature,
                Sender = encryptedSender
            };

            string jsonString = JsonSerializer.Serialize(encryptedDto);

            EncryptedMessage = $"{jsonString}";

            // Redisplay the form.
            return OnGet();
        }

        private X509Certificate2 GetCertificateWithPublicKeyForIdentity(string email)
        {
            var user = _applicationDbContext.Users.First(user => user.Email == email);
            var cert = _importExportCertificate.PemImportCertificate(user.PemPublicKey);
            return cert;
        }

        private X509Certificate2 GetCertificateWithPrivateKeyForIdentity()
        {
            var user = _applicationDbContext.Users.First(user => user.Email == User.Identity.Name);

            var cert = _importExportCertificate.PemImportCertificate(user.PemPrivateKey,
                _configuration["PemPasswordExportImport"]);

            return cert;
        }
    }
}
