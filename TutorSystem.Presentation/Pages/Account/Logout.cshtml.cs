using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Presentation.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IUserService _userService;

        public LogoutModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            await _userService.LogoutUserAsync();
            return RedirectToPage("/Index");
        }
    }
}
