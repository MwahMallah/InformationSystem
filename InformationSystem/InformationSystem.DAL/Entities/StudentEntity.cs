using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystem.DAL.Entities {
    public class StudentEntity 
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public required string Group { get; set; }
        public int CurrentYear { get; set; } 

        public Guid CourseId { get; set; }
        public ICollection<CourseEntity> ChosenCourses { get; init; } = new List<CourseEntity>();
    }
}
