using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using CertificateManager;
using EncryptDecryptLib;
using ExchangeSecureTexts.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSecureTexts.Pages
{
    public class EncryptTextModel : PageModel
    {
        private readonly SymmetricEncryptDecrypt _symmetricEncryptDecrypt;
        private readonly AsymmetricEncryptDecrypt _asymmetricEncryptDecrypt;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ImportExportCertificate _importExportCertificate;

        [BindProperty]
        [Required]
        public string TargetUserEmail { get; set; }

        [BindProperty]
        [Required]
        public string Message { get; set; }

        [BindProperty]
        public string EncryptedMessage { get; set; }

        public List<SelectListItem> Users { get; set; }

        public EncryptTextModel(SymmetricEncryptDecrypt symmetricEncryptDecrypt,
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

            var (Key, IVBase64) = _symmetricEncryptDecrypt.InitSession();

            var encryptedText = _symmetricEncryptDecrypt.Encrypt(Message, IVBase64, Key);

            var targetUserPublicCertificate = GetCertificateWithPublicKeyForIdentity(TargetUserEmail);

            var encryptedKey = _asymmetricEncryptDecrypt.Encrypt(Key,
                _asymmetricEncryptDecrypt.CreateCipherPublicKey(targetUserPublicCertificate));

            var encryptedIV = _asymmetricEncryptDecrypt.Encrypt(IVBase64,
              _asymmetricEncryptDecrypt.CreateCipherPublicKey(targetUserPublicCertificate));

            var encryptedDto = new EncryptedDto
            {
                EncryptedText = encryptedText,
                Key = encryptedKey,
                IV = encryptedIV
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
    }
}
