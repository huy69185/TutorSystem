using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TutorSystem.Repository.Entities;
using TutorSystem.Repository.Interfaces;

namespace TutorSystem.Repository.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly TutorSystemContext _context;

        public TutorRepository(TutorSystemContext context)
        {
            _context = context;
        }

        public async Task<bool> UploadTutorDocumentAsync(TutorDocument document)
        {
            _context.TutorDocuments.Add(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TutorDocument>> GetTutorDocumentsAsync(Guid tutorId)
        {
            return await _context.TutorDocuments
                .Where(d => d.TutorId == tutorId)
                .ToListAsync();
        }

        public async Task<Tutor> GetTutorByUserIdAsync(Guid userId)
        {
            return await _context.Tutors.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<bool> ApproveTutorAsync(Guid tutorId)
        {
            var tutor = await _context.Tutors.FindAsync(tutorId);
            if (tutor != null)
            {
                tutor.IsApproved = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task SaveTutorDocumentAsync(TutorDocument tutorDocument)
        {
            _context.TutorDocuments.Add(tutorDocument);
            await _context.SaveChangesAsync();
        }
    }
}
