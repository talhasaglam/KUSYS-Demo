using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace KUSYS_Demo.DataAccess.EntityConfigurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName(nameof(Course.Id));
            builder.Property(x => x.CourseCode).HasColumnName(nameof(Course.CourseCode)).IsRequired().HasMaxLength(12); ;
            builder.HasIndex(x => x.CourseCode,"UC_Course_CoursCode").IsUnique();
            builder.Property(x => x.CourseName).HasColumnName(nameof(Course.CourseName)).IsRequired().HasMaxLength(128); ;
        }
    }
}
