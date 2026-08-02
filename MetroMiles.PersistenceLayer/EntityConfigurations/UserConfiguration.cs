using Core.SecurityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(u => u.LastName).HasColumnName("LastName").IsRequired();
        // Bounded (rather than nvarchar(max)) so the NormalizedEmail computed column below also
        // resolves to a bounded type — SQL Server can't put an nvarchar(max) column in an index.
        builder.Property(u => u.Email).HasColumnName("Email").HasMaxLength(256).IsRequired();
        builder.Property(u => u.PasswordSalt).HasColumnName("PasswordSalt").IsRequired();
        builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash").IsRequired();
        builder.Property(u => u.Status).HasColumnName("Status").HasDefaultValue(true);
        builder.Property(u => u.AuthenticatorType).HasColumnName("AuthenticatorType").IsRequired();
        builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");

        // Database-computed, always-in-sync uppercase mirror of Email. Lets uniqueness/lookup
        // queries compare on a plain indexed column instead of calling ToLower()/ToUpper() inside
        // the LINQ predicate, which EF Core can't translate in a culture-safe, collation-independent way.
        builder.Property(u => u.NormalizedEmail).HasColumnName("NormalizedEmail").HasComputedColumnSql("UPPER([Email])", stored: true);
        builder.HasIndex(u => u.NormalizedEmail).HasDatabaseName("IX_Users_NormalizedEmail");

        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);

        builder.HasMany(u => u.UserOperationClaims);
        builder.HasMany(u => u.RefreshTokens);
        builder.HasMany(u => u.EmailAuthenticators);
        builder.HasMany(u => u.OtpAuthenticators);

        builder.HasData(GetSeeds());
    }

    // Fixed PBKDF2 hash/salt for the seed password "Passw0rd" (HashingHelper.CreatePasswordHash
    // output, captured once). HasData() values are baked into the migration snapshot and diffed
    // on every migration-add, so they must be deterministic — calling CreatePasswordHash here would
    // mint a fresh random salt on every build and make EF think the seed changed each time.
    private static readonly byte[] s_seedAdminPasswordHash =
    [
        215, 205, 2, 234, 175, 152, 107, 196, 193, 49, 18, 156, 181, 133, 39, 157,
        153, 101, 83, 86, 161, 89, 232, 11, 235, 40, 179, 49, 11, 15, 137, 208,
    ];

    private static readonly byte[] s_seedAdminPasswordSalt =
    [
        88, 106, 194, 164, 64, 8, 0, 18, 250, 113, 212, 117, 14, 227, 36, 196,
    ];

    private static User[] GetSeeds()
    {
        User adminUser = new()
        {
            Id = 1,
            FirstName = "Admin",
            LastName = "MetroMiles",
            Email = "admin@metromiles.com",
            Status = true,
            PasswordHash = s_seedAdminPasswordHash,
            PasswordSalt = s_seedAdminPasswordSalt
        };

        return [adminUser];
    }
}
