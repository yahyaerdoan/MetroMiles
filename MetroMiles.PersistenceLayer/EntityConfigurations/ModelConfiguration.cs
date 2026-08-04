using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("Models").HasKey(m => m.Id);

        builder.Property(m => m.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
        builder.Property(m => m.Name).HasColumnName("Name").HasMaxLength(450).IsRequired();
        builder.Property(m => m.DailyPrice).HasColumnName("DailyPrice").IsRequired().HasPrecision(18, 2);
        builder.Property(m => m.ImageUrl).HasColumnName("ImageUrl").IsRequired();

        builder.Property(m => m.FuelId).HasColumnName("FuelId").IsRequired();
        builder.Property(m => m.BrandId).HasColumnName("BrandId").IsRequired();
        builder.Property(m => m.TransmissionId).HasColumnName("TransmissionId").IsRequired();

        builder.Property(m => m.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(m => m.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(m => m.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(m => m.RowVersion).HasColumnName("RowVersion").IsRowVersion();

        // Database-computed, always-in-sync uppercase mirror of Name. Lets uniqueness/lookup
        // queries compare on a plain indexed column instead of calling ToLower()/ToUpper() inside
        // the LINQ predicate, which EF Core can't translate in a culture-safe, collation-independent way.
        builder.Property(m => m.NormalizedName).HasColumnName("NormalizedName").HasComputedColumnSql("UPPER([Name])", stored: true);

        builder.HasIndex(m => m.Name).HasDatabaseName("UK_Models_Name").IsUnique().HasFilter("[DeletedDate] IS NULL");
        builder.HasIndex(m => m.NormalizedName).HasDatabaseName("IX_Models_NormalizedName");

        builder.HasOne(m => m.Brand)
               .WithMany(b => b.Models)
               .HasForeignKey(m => m.BrandId);

        builder.HasOne(m => m.Fuel)
               .WithMany(f => f.Models)
               .HasForeignKey(m => m.FuelId);

        builder.HasOne(m => m.Transmission)
               .WithMany(t => t.Models)
               .HasForeignKey(m => m.TransmissionId);

        builder.HasMany(m => m.Cars)
               .WithOne(c => c.Model)
               .HasForeignKey(c => c.ModelId);

        builder.HasQueryFilter(m => !m.DeletedDate.HasValue);
    }
}
