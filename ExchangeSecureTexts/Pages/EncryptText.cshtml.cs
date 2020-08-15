using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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
            ApplicationDbContext applicationDbContext)
        {
            _symmetricEncryptDecrypt = symmetricEncryptDecrypt;
            _asymmetricEncryptDecrypt = asymmetricEncryptDecrypt;
            _applicationDbContext = applicationDbContext;
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

            var encryptedDto = new EncryptedDto
            {
                EncryptedText = encryptedText,
                Key = Key,
                IV = IVBase64
            };

            string jsonString = JsonSerializer.Serialize(encryptedDto);

            EncryptedMessage = $"{jsonString}";

            // Redisplay the form.
            return OnGet();

        }
    }
}
