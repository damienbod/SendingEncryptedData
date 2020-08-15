using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EncryptDecryptLib;
using ExchangeSecureTexts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Something failed. Redisplay the form.
                return OnGet();
            }
            EncryptedMessage = $"TODO Encrypt {Message}";

            // Redisplay the form.
            return OnGet();

        }
    }
}
