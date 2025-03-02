using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Presentation.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;

        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string FullName { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string University { get; set; }

        [BindProperty]
        public string Role { get; set; } = "Student"; // Mặc định là Student

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                await _userService.RegisterUserAsync(FullName, Email, Password, PhoneNumber, University, Role);

                TempData["SuccessMessage"] = "Đăng ký thành công! Hãy đăng nhập để tiếp tục.";

                return RedirectToPage("/Account/Login"); // ✅ Chuyển về trang đăng nhập
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
