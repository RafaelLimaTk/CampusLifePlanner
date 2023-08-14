using CampusLifePlanner.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusLifePlanner.Infra.Data.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email);
            builder.HasKey(x => x.Name);
            builder.HasKey(x => x.CreatedAt);
            builder.HasKey(x => x.UpdatedAt);
        }
    }
}
