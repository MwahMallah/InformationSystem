using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL {
    public class InformationSystemDbContext(DbContextOptions options) : DbContext(options) {
        public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
        public DbSet<CourseEntity> Courses => Set<CourseEntity>();
        public DbSet<EvaluationEntity> Evaluations => Set<EvaluationEntity>();
        public DbSet<StudentEntity> Students => Set<StudentEntity>();
        public DbSet<StudentCourseEntity> StudentCourseEntities => Set<StudentCourseEntity>();
    }
}
