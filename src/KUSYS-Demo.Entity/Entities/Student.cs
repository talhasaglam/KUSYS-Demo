using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KUSYS_Demo.Entity.Entities
{
    public class Student : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public ICollection<StudentCourse> Courses { get; set; }

        public Student()
        {

        }

        public Student(string firstName, string lastName, DateTime birthDate) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Courses = new Collection<StudentCourse>();

        }

        public Student(int id, string firstName, string lastName, DateTime birthDate) : this(firstName,lastName,birthDate)
        {
            Id = id;
        }

        public void AddCourse(int courseId) { 

             if (IsInCourse(courseId)) { return; }

            Courses.Add(new StudentCourse(studentId: Id, courseId: courseId));
        }

        public void RemoveCourse(int courseId)
        {

            if (!IsInCourse(courseId)) { return; }

            Courses.ToList().RemoveAll(x => x.CourseId == courseId);
        }

        private bool IsInCourse(int courseId) {

            return Courses.Any(x => x.CourseId == courseId); 

        }

        public void RemoveAllCourse(int courseId)
        {

            if (!IsInCourse(courseId)) { return; }

            Courses.ToList().RemoveAll(x => x.StudentId == Id);
        }
    }
}
