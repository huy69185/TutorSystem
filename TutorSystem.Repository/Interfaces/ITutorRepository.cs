using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;

namespace TutorSystem.Repository.Interfaces
{
    public interface ITutorRepository
    {
        Task<bool> UploadTutorDocumentAsync(TutorDocument document);
        Task<List<TutorDocument>> GetTutorDocumentsAsync(Guid tutorId);
        Task<Tutor> GetTutorByUserIdAsync(Guid userId);
        Task<bool> ApproveTutorAsync(Guid tutorId);
        Task SaveTutorDocumentAsync(TutorDocument tutorDocument);
    }
}
