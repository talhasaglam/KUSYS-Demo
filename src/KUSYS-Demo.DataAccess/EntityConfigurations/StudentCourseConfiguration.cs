using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KUSYS_Demo.DataAccess.EntityConfigurations
{
    public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
    {
        public void Configure(EntityTypeBuilder<StudentCourse> builder)
        {
            builder.HasKey(pt => new { pt.StudentId, pt.CourseId });

            builder.HasOne(pt => pt.Student)
                .WithMany(p => p.Courses)
                .HasForeignKey(pt => pt.StudentId);

            builder.HasOne(pt => pt.Course)
                .WithMany(p => p.Students)
                .HasForeignKey(pt => pt.CourseId);

            builder.HasIndex(pt => new { pt.StudentId, pt.CourseId },"UC_Student_Course_Id");
        }
    }
}
