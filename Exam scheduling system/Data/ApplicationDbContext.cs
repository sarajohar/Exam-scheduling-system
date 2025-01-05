using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

using NuGet.DependencyResolver;
using ExamSchedulingSystem.Models;

namespace ExamSchedulingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


       
        public DbSet<User> Users { get; set; }
        public DbSet<ExamSchedulingSystem.Models.Coordinator> Coordinators { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Invigilator> Invigilators { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassRoom> ClassRooms { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<ExamReservation> ExamReservations { get; set; }
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<Excuse> Excuses { get; set; }
        public DbSet<ChangeRequest> ChangeRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Calendar> Calendars { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<User>()
           .Property(u => u.Role)
           .HasConversion<string>();

            
            modelBuilder.Entity<User>()
                .Property(u => u.FacultyRole)
                .HasConversion<string>();
            modelBuilder.Entity<ExamReservation>()
          .Property(e => e.ExamType)
          .HasConversion<string>();

           

            // Coordinator - User relationship
            modelBuilder.Entity<ExamSchedulingSystem.Models.Coordinator>()
                .HasOne(c => c.User)
                .WithMany(u => u.Coordinators)
                .HasForeignKey(c => c.UserId);

            // Teacher - User relationship
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.User)
                .WithMany(u => u.Teachers)
                .HasForeignKey(t => t.UserId);

            // Invigilator - User relationship
            modelBuilder.Entity<Invigilator>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invigilators)
                .HasForeignKey(i => i.UserId);

            // Student - User relationship (One-to-one relationship)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.UserId);
        }

    }

}