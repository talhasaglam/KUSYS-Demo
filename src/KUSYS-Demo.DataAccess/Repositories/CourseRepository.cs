using KUSYS_Demo.DataAccess.Contexts;
using KUSYS_Demo.DataAccess.Repositories.İnterfaces;
using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace KUSYS_Demo.DataAccess.Repositories
{
    public class CourseRepository: EfCoreRepository<Course,BaseDbContext>, ICourseRepository
    {
        public CourseRepository(BaseDbContext dbContext): base(dbContext)
        {
            
        }

    }
}
