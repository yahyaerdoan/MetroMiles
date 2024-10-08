﻿using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class FuelConfiguration : IEntityTypeConfiguration<Fuel>
{
    public void Configure(EntityTypeBuilder<Fuel> builder)
    {
        builder.ToTable("Fuels").HasKey(f => f.Id);

        builder.Property(f => f.Id).HasColumnName("Id").IsRequired();

        builder.Property(f => f.Name).HasColumnName("Name").IsRequired();

        builder.Property(f => f.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(f => f.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(f => f.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(indexExpression => indexExpression.Name, name: "UK_Fuels_Name").IsUnique();

        builder.HasMany(f => f.Models)
            .WithOne(m => m.Fuel)
            .HasForeignKey(m => m.FuelId);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    } 
}
