using CampusLifePlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusLifePlanner.Infra.Data.EntitiesConfiguration;

public class EnrollmentCourseConfiguration : IEntityTypeConfiguration<EnrollmentCourse>
{
    public void Configure(EntityTypeBuilder<EnrollmentCourse> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(x => x.UserId);
        builder.Property(x => x.CourseId);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(e => e.CourseId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}
