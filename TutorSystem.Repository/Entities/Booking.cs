using System;
using System.Collections.Generic;

namespace TutorSystem.Repository.Entities;

public partial class Booking
{
    public Guid BookingId { get; set; }

    public Guid StudentId { get; set; }

    public Guid TutorId { get; set; }

    public Guid SubjectId { get; set; }

    public DateTime BookingDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Status { get; set; }

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Review? Review { get; set; }

    public virtual User Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
