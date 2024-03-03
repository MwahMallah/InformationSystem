using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL 
{
    public class InformationSystemDbContext(DbContextOptions options) : DbContext(options) {
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<CourseEntity> Courses => Set<CourseEntity>();
        public DbSet<EvaluationEntity> Evaluations => Set<EvaluationEntity>();
        public DbSet<StudentEntity> Students => Set<StudentEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourseEntity>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<StudentCourseEntity>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<StudentCourseEntity>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);
            
            modelBuilder.Entity<ActivityEntity>()
                .HasOne(a => a.Evaluation)
                .WithOne(e => e.Activity);

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Activities)
                .WithOne(a => a.Course);

            modelBuilder.Entity<EvaluationEntity>()
                .HasOne(e => e.Student);
        }
    }
}
