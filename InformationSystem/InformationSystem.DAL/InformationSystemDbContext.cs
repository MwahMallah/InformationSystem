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
            
            modelBuilder.Entity<ActivityEntity>()
                .HasOne(a => a.Evaluation)
                .WithOne(e => e.Activity);

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Activities)
                .WithOne(a => a.Course);

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity(j => j.ToTable("StudentCourse"));

            modelBuilder.Entity<EvaluationEntity>()
                .HasOne(e => e.Student);
        }
    }
}
