using KUSYS_Demo.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KUSYS_Demo.DataAccess.EntityConfigurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        //Cons olarak hasmaxlengthler yazılabilir.
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students").HasKey(k => k.Id);
            builder.Property(x => x.Id).HasColumnName(nameof(Student.Id));
            builder.Property(x => x.FirstName).HasColumnName(nameof(Student.FirstName)).IsRequired().HasMaxLength(64);
            builder.Property(x => x.LastName).HasColumnName(nameof(Student.LastName)).IsRequired().HasMaxLength(64);
            builder.Property(x => x.BirthDate).HasColumnName(nameof(Student.BirthDate)).IsRequired();

            //builder.HasMany(x => x.Courses).WithOne(x => x.Student).HasForeignKey(x => x.StudentId);

        }
    }
}
