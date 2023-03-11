using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
