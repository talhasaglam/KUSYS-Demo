using KUSYS_Demo.DataAccess.Repositories.İnterfaces;
using KUSYS_Demo.Entity.Entities;
using KUSYS_Demo.Entity.Types;

namespace KUSYS_Demo.DataAccess.DataSeeder
{
    public class CourseDataSeeder : BaseDataSeeder
    {
        private readonly ICourseRepository _courseRepository;
        public CourseDataSeeder(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public override async Task SeedAsync()
        {
            (int codeNumber, CourseType courseType,string Name)[] entities = new [] {(101,CourseType.ComputerScience, "Introduction to Computer Science22"),
                                                           (102, CourseType.ComputerScience, "Algorithms"),
                                                           (101,CourseType.Maths, "Calculus"),
                                                           (101,CourseType.Physics, "Physics")};
            
            foreach (var (CodeNumber, CourseType, Name) in entities)
            {
                await SeedSingleCourseAsync(CodeNumber, CourseType, Name);
            }
        }

        private async Task SeedSingleCourseAsync(int codeNumber, CourseType courseType, string name)
        {
            var course = await _courseRepository.GetAsync(x => x.CourseCode == $"{courseType.ToDescriptionString()}{codeNumber}" );

            if (course is null)
                await _courseRepository.InsertAsync(new Course(codeNumber, courseType, name), true);
            else if(course.CourseName != name)
            {
                course.CourseName = name;
                await _courseRepository.UpdateAsync(course, true);
            }
        }
    }
}
