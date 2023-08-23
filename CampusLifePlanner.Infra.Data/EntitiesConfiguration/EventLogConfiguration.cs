using CampusLifePlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusLifePlanner.Infra.Data.EntitiesConfiguration;

public class EventLogConfiguration : IEntityTypeConfiguration<EventLog>
{
    public void Configure(EntityTypeBuilder<EventLog> builder)
    {
        builder.HasOne(e => e.Event)
               .WithMany(e => e.EventLogs)
               .HasForeignKey(e => e.EventId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
