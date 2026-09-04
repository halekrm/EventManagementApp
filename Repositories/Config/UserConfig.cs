using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.EncryptedPassword)
                .HasMaxLength(1024)
                .IsRequired();

            builder.Property(x => x.BirthDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(x => x.Role)
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("User");

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}