using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("Fuels").HasKey(m => m.Id);
        builder.Property(m => m.Id).HasColumnName("Id").IsRequired();
        builder.Property(m => m.Name).HasColumnName("Name").IsRequired();
        builder.Property(m => m.DailyPrice).HasColumnName("DailyPrice").IsRequired();
        builder.Property(m => m.ImageUrl).HasColumnName("ImageUrl").IsRequired();

        builder.Property(m => m.FuelId).HasColumnName("FuelId").IsRequired();
        builder.Property(m => m.BrandId).HasColumnName("BrandId").IsRequired();
        builder.Property(m => m.TransmissionId).HasColumnName("TransmissionId").IsRequired();

        builder.Property(m => m.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(m => m.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(m => m.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(indexExpression => indexExpression.Name, name: "UK_Models_Name").IsUnique();

        builder.HasOne(m => m.Brand);
        builder.HasOne(m => m.Fuel);
        builder.HasOne(m => m.Transmission);
        builder.HasMany(m => m.Cars);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}
