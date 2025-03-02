using System;
using System.Collections.Generic;

namespace TutorSystem.Repository.Entities;

public partial class Subject
{
    public Guid SubjectId { get; set; }

    public string SubjectName { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
}
