using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class TransmissionConfiguration : IEntityTypeConfiguration<Transmission>
{
    public void Configure(EntityTypeBuilder<Transmission> builder)
    {
        builder.ToTable("Transmissions").HasKey(f => f.Id);

        // NEWSEQUENTIALID() backstops EF Core's client-side sequential-GUID generation (already the
        // SQL Server provider default when Id is left unset) at the DB level too, so the clustered
        // index still avoids random-GUID fragmentation even for rows inserted outside of EF Core.
        builder.Property(f => f.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
        builder.Property(f => f.Name).HasColumnName("Name").HasMaxLength(450).IsRequired();

        builder.Property(f => f.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(f => f.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(f => f.DeletedDate).HasColumnName("DeletedDate");

        // Database-computed, always-in-sync uppercase mirror of Name. Lets uniqueness/lookup
        // queries compare on a plain indexed column instead of calling ToLower()/ToUpper() inside
        // the LINQ predicate, which EF Core can't translate in a culture-safe, collation-independent way.
        builder.Property(f => f.NormalizedName).HasColumnName("NormalizedName").HasComputedColumnSql("UPPER([Name])", stored: true);

        builder.HasIndex(indexExpression => indexExpression.Name, name: "UK_Transmissions_Name").IsUnique();
        builder.HasIndex(f => f.NormalizedName).HasDatabaseName("IX_Transmissions_NormalizedName");

        builder.HasMany(f => f.Models);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
