using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.School.Persistence.SQL.EntityConfigurations
{
    public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses", "dbo");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Title)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasIndex(r => r.Title)
                .IsUnique();
        }
    }
}