using Microsoft.Extensions.DependencyInjection;

namespace KUSYS_Demo.DataAccess.DataSeeder
{
    public static class DataSeederServiceProvider
    {
        public static IServiceProvider DataSeed(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var contexting = services.GetRequiredService<DataSeederContributor>();

                contexting?.SeedAsync().Wait();
            }

            return serviceProvider;
        }
    }
}
