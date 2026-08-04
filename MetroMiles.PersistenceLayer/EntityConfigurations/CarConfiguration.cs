using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars").HasKey(c => c.Id);

        builder.Property(c => c.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();

        builder.Property(c => c.Kilometer).HasColumnName("Kilometer").IsRequired();
        builder.Property(c => c.Mile).HasColumnName("Mile").IsRequired();
        builder.Property(c => c.ModelYear).HasColumnName("ModelYear").IsRequired();
        builder.Property(c => c.Plate).HasColumnName("Plate").HasMaxLength(20).IsRequired();
        builder.Property(c => c.NormalizedPlate).HasColumnName("NormalizedPlate").HasComputedColumnSql("UPPER([Plate])", stored: true);
        builder.Property(c => c.MinFindexScore).HasColumnName("MinFindexScore").IsRequired();
        builder.Property(c => c.Status).HasColumnName("Status").IsRequired();

        builder.Property(c => c.ModelId).HasColumnName("ModelId").IsRequired();

        builder.Property(c => c.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(c => c.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(c => c.DeletedDate).HasColumnName("DeletedDate");
        builder.Property(c => c.RowVersion).HasColumnName("RowVersion").IsRowVersion();

        builder.HasIndex(c => c.Plate, name: "UK_Cars_Plate").IsUnique().HasFilter("[DeletedDate] IS NULL");
        builder.HasIndex(c => c.NormalizedPlate).HasDatabaseName("IX_Cars_NormalizedPlate");

        builder.HasOne(c => c.Model)
            .WithMany(m => m.Cars)
            .HasForeignKey(c => c.ModelId);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
