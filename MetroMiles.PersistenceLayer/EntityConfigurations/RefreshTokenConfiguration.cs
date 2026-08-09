using MetroMiles.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens").HasKey(rt => rt.Id);

        builder.Property(rt => rt.Id).HasColumnName("Id").HasDefaultValueSql("NEWSEQUENTIALID()").IsRequired();
        builder.Property(rt => rt.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(rt => rt.Token).HasColumnName("Token").HasMaxLength(64).IsRequired();
        builder.Property(rt => rt.Expires).HasColumnName("Expires").IsRequired();
        builder.Property(rt => rt.CreatedByIp).HasColumnName("CreatedByIp").HasMaxLength(64).IsRequired();
        builder.Property(rt => rt.Revoked).HasColumnName("Revoked");
        builder.Property(rt => rt.RevokedByIp).HasColumnName("RevokedByIp").HasMaxLength(64);
        builder.Property(rt => rt.ReplacedByToken).HasColumnName("ReplacedByToken").HasMaxLength(64);

        builder.Property(rt => rt.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(rt => rt.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(rt => rt.DeletedDate).HasColumnName("DeletedDate");
        builder.Ignore(rt => rt.RowVersion);

        builder.HasIndex(rt => rt.Token).HasDatabaseName("IX_RefreshTokens_Token");

        builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rt => rt.UserId);

        builder.HasQueryFilter(rt => !rt.DeletedDate.HasValue);
    }
}
