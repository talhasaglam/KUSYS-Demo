using KUSYS_Demo.DataAccess.Repositories.Ýnterfaces;
using KUSYS_Demo.Entity.Entities;
using Moq;

namespace KUSYS_Demo.DataAccess.Tests
{
    public class CourseRepositoryTests
    {
        private readonly Mock<ICourseRepository> _courseRepository;
        public CourseRepositoryTests()
        {
            _courseRepository = new();
        }

        [Fact]
        public async Task GetCourseList()
        {
            var courseList = CourseListData();
            _courseRepository.Setup(x => x.GetListAsync(null,default).Result).Returns(courseList);

            var repo = _courseRepository.Object;
            var resultList = await repo.GetListAsync();

            Assert.NotNull(resultList);
            Assert.NotNull(resultList);
            Assert.Equal(courseList.Count(), resultList.Count());
            Assert.Equal(courseList.ToString(), resultList.ToString());
            Assert.True(courseList.Equals(resultList));

        }


        [Fact]
        public async Task GetCourse()
        {
            var courseList = CourseListData();
            _courseRepository.Setup(x => x.GetAsync(x=>x.Id==1,null,default).Result).Returns(courseList[0]);

            var repo = _courseRepository.Object;
            var resultList = await repo.GetAsync(x=>x.Id==1);

            Assert.NotNull(resultList);
            Assert.Equal(courseList[0].CourseCode, resultList.CourseCode);
            Assert.True(courseList[0].CourseName == resultList.CourseName);

        }

        private List<Course> CourseListData()
        {
            return new List<Course> { new Course(1,101,Entity.Types.CourseType.ComputerScience, "Introduction to Computer Science"),
                                      new Course(2,102,Entity.Types.CourseType.ComputerScience, "Algorithms"),
                                      new Course(3,101,Entity.Types.CourseType.Maths, "Calculus") };
        }
    }
}