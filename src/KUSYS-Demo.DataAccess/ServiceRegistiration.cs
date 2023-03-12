using KUSYS_Demo.DataAccess.Contexts;
using KUSYS_Demo.DataAccess.Repositories.Interfaces;
using KUSYS_Demo.DataAccess.Repositories.İnterfaces;
using KUSYS_Demo.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KUSYS_Demo.DataAccess
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            //DbConxtect Initialize with Connection String
            services.AddDbContext<BaseDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("KusysDemoConnectionString"));
                //options.EnableSensitiveDataLogging();
            });

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();

            return services;
        }
    }
}
