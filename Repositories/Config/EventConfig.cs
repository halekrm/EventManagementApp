using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{

    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.EventId);

            builder.Property(x => x.Title)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.StartDateTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(x => x.EndDateTime)
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(x => x.ImagePath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.ShortDescription)
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(x => x.LongDescription)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CreatedByUserId, x.Title })
                .IsUnique();
        }
    }
}