using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.Entity.Entities
{
    public class Course : BaseEntity
    {
        public string CourseCode { get; set; }
        public string CourseName { get; set; }

        public ICollection<StudentCourse> Students { get; set; }

        public Course()
        {

        }

        public Course(string courseCode, string courseName) : this()
        {
            CourseCode = courseCode;
            CourseName = courseName;
            Students = new Collection<StudentCourse>();
        }

    }
}
