using KUSYS_Demo.DataAccess.Repositories;
using KUSYS_Demo.DataAccess.Repositories.İnterfaces;
using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.Entity.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.DataAccess.Tests
{
    public class StudentRepositoryTests
    {
        private readonly Mock<IStudentRepository> _studentRepository;
        public StudentRepositoryTests()
        {
            _studentRepository = new();
        }

        [Fact]
        public async Task CreateStudent()
        {
            var newStudent = new Student(4, "Emre", "Saglam", Convert.ToDateTime("17/02/1995"));
            _studentRepository.Setup(x => x.InsertAsync(newStudent, true,default).Result).Returns(newStudent);

            var repo = _studentRepository.Object;
            var result = await repo.InsertAsync(newStudent,true);

            Assert.NotNull(result);
            Assert.Equal(newStudent.FirstName, result.FirstName);
        }

        [Fact]
        public async Task UpdateStudent()
        {
            var updatedStudent = new Student(3, "Emre", "Saglam", Convert.ToDateTime("17/02/1995"));
            _studentRepository.Setup(x => x.UpdateAsync(updatedStudent, true, default).Result).Returns(updatedStudent);

            var repo = _studentRepository.Object;
            var result = await repo.UpdateAsync(updatedStudent,true);

            Assert.NotNull(result);
            Assert.Equal(updatedStudent.Courses.Count, result.Courses.Count);
        }

    }
}
