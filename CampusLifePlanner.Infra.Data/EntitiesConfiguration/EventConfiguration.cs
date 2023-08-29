using CampusLifePlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusLifePlanner.Infra.Data.EntitiesConfiguration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.Description)
           .IsRequired(false)
           .HasMaxLength(500);

        builder.Property(e => e.Local)
            .IsRequired()
            .HasMaxLength(120);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.JobId)
            .IsRequired(false)
            .HasMaxLength(4000);

        builder.HasOne(e => e.Course)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CourseId)
            .IsRequired();
    }
}
