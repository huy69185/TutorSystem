using System;
using System.Collections.Generic;

namespace TutorSystem.Repository.Entities;

public partial class Tutor
{
    public Guid TutorId { get; set; }

    public Guid UserId { get; set; }

    public string? Bio { get; set; }

    public decimal HourlyRate { get; set; }

    public double? Rating { get; set; }

    public int? ReviewsCount { get; set; }

    public string? AvailableHours { get; set; }

    public bool? IsApproved { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<TutorDocument> TutorDocuments { get; set; } = new List<TutorDocument>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
