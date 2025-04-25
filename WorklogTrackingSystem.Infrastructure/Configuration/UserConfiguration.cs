using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Infrastructure.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.Login).IsRequired(); // Consider adding HasIndex().IsUnique() if needed
            builder.Property(e => e.PasswordHash).IsRequired();
            builder.Property(e => e.Role).IsRequired();
            builder.Property(e => e.DailyMinHours).IsRequired();
            builder.Property(e => e.DailyMaxHours).IsRequired();
            builder.Property(e => e.RefreshToken).IsRequired(false); // Explicitly optional
            builder.Property(e => e.RefreshTokenExpiryTime).IsRequired(false);
        }
    }
}
