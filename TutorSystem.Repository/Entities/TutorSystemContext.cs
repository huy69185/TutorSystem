using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TutorSystem.Repository.Entities;

public partial class TutorSystemContext : DbContext
{
    public TutorSystemContext(DbContextOptions<TutorSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Tutor> Tutors { get; set; }
    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Booking> Bookings { get; set; }
    public virtual DbSet<Review> Reviews { get; set; }
    public virtual DbSet<Subject> Subjects { get; set; }
    public virtual DbSet<TutorDocument> TutorDocuments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TutorDocument>()
            .HasKey(d => d.DocumentId); // Định nghĩa khóa chính

        modelBuilder.Entity<Tutor>()
            .HasMany(t => t.TutorDocuments)
            .WithOne(d => d.Tutor)
            .HasForeignKey(d => d.TutorId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa tài liệu nếu Tutor bị xóa

        base.OnModelCreating(modelBuilder);
    }
}
