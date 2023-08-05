using CampusLifePlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusLifePlanner.Infra.Data.EntitiesConfiguration;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired();

        builder.HasMany(c => c.Events)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId);
    }
}
