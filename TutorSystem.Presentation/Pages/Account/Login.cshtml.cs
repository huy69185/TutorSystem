using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Presentation.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITutorService _tutorService;

        public LoginModel(IUserService userService, ITutorService tutorService)
        {
            _userService = userService;
            _tutorService = tutorService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var loginRedirectPage = await _userService.LoginUserAsync(Email, Password);

            if (loginRedirectPage != null)
            {
                // 🔥 Nếu tutor chưa được xác minh, chuyển hướng ngay tại đây
                if (loginRedirectPage == "/Account/TutorVerification")
                {
                    return RedirectToPage("/Account/TutorVerification");
                }

                return RedirectToPage(loginRedirectPage);
            }

            ErrorMessage = "Email hoặc mật khẩu không đúng!";
            return Page();
        }
    }
}
