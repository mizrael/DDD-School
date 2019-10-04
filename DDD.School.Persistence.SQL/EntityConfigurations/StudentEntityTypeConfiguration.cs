using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.School.Persistence.SQL.EntityConfigurations
{
    public class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students", "dbo");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Firstname)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(r => r.Firstname)
                .HasMaxLength(250)
                .IsRequired();

            //https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities
            builder.OwnsMany(r => r.Courses, b =>
            {
                b.ToTable("StudentCourses", "dbo")
                    .HasKey(ur => new { StudentId = ur.StudentId, CourseId = ur.CourseId });

                b.Property(ur => ur.StudentId)
                    .UsePropertyAccessMode(PropertyAccessMode.Property);

                b.Property(ur => ur.CourseId)
                    .UsePropertyAccessMode(PropertyAccessMode.Property);
            });

            var nav = builder.Metadata.FindNavigation(nameof(Student.Courses));
            nav.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}