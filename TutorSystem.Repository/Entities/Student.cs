using System;
using System.Collections.Generic;

namespace TutorSystem.Repository.Entities;

public partial class Student
{
    public Guid StudentId { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
