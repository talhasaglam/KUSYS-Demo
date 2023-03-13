using KUSYS_Demo.Entity.Types;
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
        public string CourseCode { get; private set; }
        public string CourseName { get; set; }

        public ICollection<StudentCourse> Students { get; set; }

        public Course()
        {

        }

        public Course(int courseNumber,CourseType courseType, string courseName) : this()
        {
            CourseCode = GetPrefix(courseNumber,courseType);
            CourseName = courseName;
            Students = new Collection<StudentCourse>();
        }

        public Course(int id,int courseNumber, CourseType courseType, string courseName) : this(courseNumber, courseType, courseName)
        {
            Id = id;
        }

        public void SetCourseCode(int courseNumber, CourseType courseType)
        {
            CourseCode = GetPrefix(courseNumber, courseType);
        }
        private static string GetPrefix(int courseNumber, CourseType courseType)
        {
            return courseType.ToDescriptionString() + courseNumber.ToString();
        }

    }
}
