using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationSystem.DAL.Entities {
    public class StudentCourseEntity 
    {
        public Guid Id { get; set; }
        public int Attempt { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }   
    }
}
