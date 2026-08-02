using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands").HasKey(b => b.Id);

        // NEWSEQUENTIALID() backstops EF Core's client-side sequential-GUID generation (already the
        // SQL Server provider default when Id is left unset) at the DB level too, so the clustered
        // index still avoids random-GUID fragmentation even for rows inserted outside of EF Core.
        builder.Property(b => b.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
        builder.Property(b => b.Name).HasColumnName("Name").HasMaxLength(450).IsRequired();
        builder.Property(b => b.Description).HasColumnName("Description").IsRequired();

        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        // Database-computed, always-in-sync uppercase mirror of Name. Lets uniqueness/lookup
        // queries compare on a plain indexed column instead of calling ToLower()/ToUpper() inside
        // the LINQ predicate, which EF Core can't translate in a culture-safe, collation-independent way.
        builder.Property(b => b.NormalizedName).HasColumnName("NormalizedName").HasComputedColumnSql("UPPER([Name])", stored: true);

        builder.HasIndex(indexExpression => indexExpression.Name, name: "UK_Brands_Name").IsUnique();
        builder.HasIndex(b => b.NormalizedName).HasDatabaseName("IX_Brands_NormalizedName");

        builder.HasMany(b => b.Models);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
