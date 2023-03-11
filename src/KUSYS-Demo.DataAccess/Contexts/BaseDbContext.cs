using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace KUSYS_Demo.DataAccess.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        // if you added entity, CratedDate filled Before SaveCahnges 
        // if you updated entity, UpdatedDate filled Before SaveChanges. Every update will change UpdatedDate.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            IEnumerable<EntityEntry<BaseEntity>> entries = ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                _ = entry.State switch
                {
                    EntityState.Added => entry.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => entry.Entity.UpdatedDate = DateTime.UtcNow
                };
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //For EntityConfiguration. It find all IEntityTypeConfiguration's
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
