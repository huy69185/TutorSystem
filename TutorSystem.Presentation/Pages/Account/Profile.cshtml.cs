using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TutorSystem.Repository.Entities;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Presentation.Pages.Account
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User InputUser { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            InputUser = await _userService.GetUserProfileAsync(Guid.Parse(userId));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("/Account/Login");

            if (!ModelState.IsValid) // ✅ Kiểm tra dữ liệu hợp lệ
                return Page();

            var existingUser = await _userService.GetUserProfileAsync(Guid.Parse(userId));

            existingUser.FullName = InputUser.FullName;
            existingUser.PhoneNumber = InputUser.PhoneNumber;
            existingUser.University = InputUser.University;

            await _userService.UpdateUserProfileAsync(existingUser);

            // ✅ Sử dụng JavaScript `localStorage` để hiển thị thông báo
            TempData["ShowPopup"] = true;

            return RedirectToPage(); // ✅ Reload lại trang để cập nhật UI
        }
    }
}
