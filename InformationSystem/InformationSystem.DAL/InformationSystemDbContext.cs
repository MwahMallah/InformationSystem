using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Seeds;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL 
{
    public class InformationSystemDbContext(DbContextOptions options, bool seedDemoData=false) 
        : DbContext(options) {
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<CourseEntity> Courses => Set<CourseEntity>();
        public DbSet<EvaluationEntity> Evaluations => Set<EvaluationEntity>();
        public DbSet<StudentEntity> Students => Set<StudentEntity>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ActivityEntity>()
                .HasMany(a => a.Evaluations)
                .WithOne(e => e.Activity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Activities)
                .WithOne(a => a.Course);

            modelBuilder.Entity<CourseEntity>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity(j => j.ToTable("StudentCourse"));

            modelBuilder.Entity<EvaluationEntity>()
                .HasOne(e => e.Student);

            if (seedDemoData)
            {
                StudentSeeds.Seed(modelBuilder);
                CourseSeeds.Seed(modelBuilder);
            }
        }
    }
}
