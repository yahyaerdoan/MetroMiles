using Core.SecurityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class OneTimePasswordAuthenticatorConfiguration : IEntityTypeConfiguration<OneTimePasswordAuthenticator>
{
    public void Configure(EntityTypeBuilder<OneTimePasswordAuthenticator> builder)
    {
        builder.ToTable("OtpAuthenticators").HasKey(oa => oa.Id);

        builder.Property(oa => oa.Id).HasColumnName("Id").IsRequired();
        builder.Property(oa => oa.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(oa => oa.SecretKey).HasColumnName("SecretKey").IsRequired();
        builder.Property(oa => oa.IsVerified).HasColumnName("IsVerified").IsRequired();
        builder.Property(oa => oa.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oa => oa.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(oa => oa.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(oa => !oa.DeletedDate.HasValue);

        builder.HasOne(oa => oa.User);
    }
}