using System;
using System.Collections.Generic;

namespace TutorSystem.Repository.Entities;

public partial class Review
{
    public Guid ReviewId { get; set; }

    public Guid BookingId { get; set; }

    public Guid StudentId { get; set; }

    public Guid TutorId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual User Student { get; set; } = null!;

    public virtual Tutor Tutor { get; set; } = null!;
}
