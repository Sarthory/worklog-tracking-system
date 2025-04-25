using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Infrastructure.Configuration
{
    public class WorklogConfiguration : IEntityTypeConfiguration<Worklog>
    {
        public void Configure(EntityTypeBuilder<Worklog> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.WorkedHours).IsRequired();
            builder.Property(e => e.TaskId).IsRequired(false);
            builder.Property(e => e.Description).IsRequired(false);

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(w => w.UserId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
