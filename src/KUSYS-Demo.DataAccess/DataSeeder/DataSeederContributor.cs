using Microsoft.Extensions.Logging;

namespace KUSYS_Demo.DataAccess.DataSeeder
{
    public class DataSeederContributor : BaseDataSeeder
    {
        private readonly CourseDataSeeder _courseDataSeeder;
        private readonly ILogger<DataSeederContributor> _logger;
        public DataSeederContributor(CourseDataSeeder courseDataSeeder, ILogger<DataSeederContributor> logger)
        {
            _courseDataSeeder = courseDataSeeder;
            _logger = logger;
        }
        public override async Task SeedAsync()
        {
            _logger.LogInformation("Seeding Courses...");
            await _courseDataSeeder.SeedAsync();
        }
    }
}
