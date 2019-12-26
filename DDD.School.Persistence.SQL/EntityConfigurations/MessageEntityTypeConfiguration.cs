using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.School.Persistence.SQL.EntityConfigurations
{

    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages", "dbo");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.ProcessedAt);

            builder.Property(r => r.Type)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(r => r.Payload)
                .IsRequired();
        }
    }
}