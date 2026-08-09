using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

// Identity's own IdentityUserConfiguration<TUser,...> (applied by IdentityDbContext.OnModelCreating)
// already configures the AspNetUsers table, keys, and its own properties — this only adds the
// MetroMiles-specific columns/conventions layered on top.
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
