using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;
using TutorSystem.Repository.Interfaces;
using TutorSystem.Service.Interfaces;

namespace TutorSystem.Service.Services
{
    public class TutorService : ITutorService
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly IUserRepository _userRepository;

        public TutorService(ITutorRepository tutorRepository, IUserRepository userRepository)
        {
            _tutorRepository = tutorRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> UploadTutorDocumentAsync(Guid tutorId, string documentType, string filePath)
        {
            var document = new TutorDocument
            {
                DocumentId = Guid.NewGuid(),
                TutorId = tutorId,
                DocumentType = documentType,
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            return await _tutorRepository.UploadTutorDocumentAsync(document);
        }

        public async Task<bool> IsTutorApprovedAsync(Guid userId)
        {
            var tutor = await _tutorRepository.GetTutorByUserIdAsync(userId);
            return tutor?.IsApproved ?? false;
        }

        public async Task<Tutor> GetTutorByUserIdAsync(Guid userId)
        {
            var tutor = await _tutorRepository.GetTutorByUserIdAsync(userId);
            if (tutor != null)
            {
                tutor.IsApproved ??= false;
            }
            return tutor;
        }

        public async Task SubmitTutorVerificationAsync(Guid tutorId, string documentType, string filePath)
        {
            var tutorDocument = new TutorDocument
            {
                DocumentId = Guid.NewGuid(),
                TutorId = tutorId,
                DocumentType = documentType, // 🔥 Lấy giá trị từ form
                FilePath = filePath,
                UploadedAt = DateTime.UtcNow
            };

            await _tutorRepository.SaveTutorDocumentAsync(tutorDocument);
        }
    }
}
