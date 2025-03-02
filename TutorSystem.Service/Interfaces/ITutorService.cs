using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;

namespace TutorSystem.Service.Interfaces
{
    public interface ITutorService
    {
        Task<bool> UploadTutorDocumentAsync(Guid tutorId, string documentType, string filePath);
        Task<bool> IsTutorApprovedAsync(Guid userId);
        Task<Tutor> GetTutorByUserIdAsync(Guid userId);
        Task SubmitTutorVerificationAsync(Guid tutorId, string documentType, string filePath);
    }
}
