using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TutorSystem.Repository.Entities;

public class TutorDocument
{
    [Key] // Xác định đây là khóa chính
    public Guid DocumentId { get; set; }

    [Required]
    [ForeignKey("Tutor")]
    public Guid TutorId { get; set; }
    public virtual Tutor Tutor { get; set; }

    [Required]
    [MaxLength(50)]
    public string DocumentType { get; set; }

    [Required]
    [MaxLength(255)]
    public string FilePath { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}