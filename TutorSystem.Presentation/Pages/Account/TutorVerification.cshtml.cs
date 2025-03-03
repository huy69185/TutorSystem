using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Threading.Tasks;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Presentation.Pages.Account
{
    public class TutorVerificationModel : PageModel
    {
        private readonly ITutorService _tutorService;

        public bool IsPending { get; set; } = false;

        public TutorVerificationModel(ITutorService tutorService)
        {
            _tutorService = tutorService;
        }

        public void OnGet(bool pending = false)
        {
            IsPending = pending;
        }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public string SelectedDocumentType { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng chọn một tệp để tải lên.");
                return Page();
            }

            if (string.IsNullOrEmpty(SelectedDocumentType) ||
                !new[] { "Degree", "Transcript", "Certificate" }.Contains(SelectedDocumentType))
            {
                ModelState.AddModelError(string.Empty, "Loại tài liệu không hợp lệ.");
                return Page();
            }

            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var tutor = await _tutorService.GetTutorByUserIdAsync(userId);

            if (tutor == null)
            {
                return RedirectToPage("/Index");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var fileName = $"{Guid.NewGuid()}_{UploadedFile.FileName}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            await _tutorService.SubmitTutorVerificationAsync(tutor.TutorId, SelectedDocumentType, $"/uploads/{fileName}");

            TempData["SuccessMessage"] = "Tài liệu xác minh đã được gửi. Vui lòng chờ xét duyệt!";

            // 🔥 Chuyển hướng ngay sau khi upload để hiển thị popup
            return RedirectToPage("/Account/TutorVerification", new { pending = true });
        }
    }
}
