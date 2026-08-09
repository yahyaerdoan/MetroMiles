using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

// Adds MetroMiles-specific columns on top of Identity's own AspNetUsers configuration.
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName).HasColumnName("FirstName").HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasColumnName("LastName").HasMaxLength(100).IsRequired();
        builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(u => u.RowVersion).HasColumnName("RowVersion").IsRowVersion();

        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);
    }
}
