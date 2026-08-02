using Core.SecurityLayer.Constants;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Features.Brands.Constants;
using MetroMiles.ApplicationLayer.Features.Cars.Constants;
using MetroMiles.ApplicationLayer.Features.Fuels.Constants;
using MetroMiles.ApplicationLayer.Features.Transmissions.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MetroMiles.PersistenceLayer.EntityConfigurations;

public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
{
    public void Configure(EntityTypeBuilder<OperationClaim> builder)
    {
        builder.ToTable("OperationClaims").HasKey(oc => oc.Id);

        builder.Property(oc => oc.Id).HasColumnName("Id").IsRequired();
        builder.Property(oc => oc.Name).HasColumnName("Name").IsRequired();
        builder.Property(oc => oc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oc => oc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(oc => oc.DeletedDate).HasColumnName("DeletedDate");
        builder.Ignore(oc => oc.RowVersion);

        builder.HasQueryFilter(oc => !oc.DeletedDate.HasValue);

        builder.HasMany(oc => oc.UserOperationClaims);

        builder.HasData(Seeds);
    }

    // Explicit, hand-maintained (Id, Name) pairs — deliberately NOT built by reflecting over
    // assembly types. That approach (removed) assigned Ids by enumeration order, which .NET does
    // not guarantee is stable: the same code could seed different Ids on different machines/builds,
    // silently reassigning what an already-seeded Id means (e.g. "brands.admin" -> "transmissions.admin").
    //
    // Rule for adding a new claim: APPEND it at the end with the next unused Id. Never reuse, reorder,
    // or renumber existing entries — UserOperationClaim rows reference these Ids, and HasData() diffs
    // against them by Id on every migration.
    private static readonly (int Id, string Name)[] s_claims =
    [
        (1, GeneralOperationClaims.Admin),

        (2, BrandsOperationClaims.Add),
        (3, BrandsOperationClaims.Admin),
        (4, BrandsOperationClaims.Delete),
        (5, BrandsOperationClaims.Read),
        (6, BrandsOperationClaims.Update),
        (7, BrandsOperationClaims.Write),

        (8, CarsOperationClaims.Add),
        (9, CarsOperationClaims.Admin),
        (10, CarsOperationClaims.Delete),
        (11, CarsOperationClaims.Read),
        (12, CarsOperationClaims.Update),
        (13, CarsOperationClaims.Write),

        (14, FuelsOperationClaims.Add),
        (15, FuelsOperationClaims.Admin),
        (16, FuelsOperationClaims.Delete),
        (17, FuelsOperationClaims.Read),
        (18, FuelsOperationClaims.Update),
        (19, FuelsOperationClaims.Write),

        (20, TransmissionsOperationClaims.Add),
        (21, TransmissionsOperationClaims.Admin),
        (22, TransmissionsOperationClaims.Delete),
        (23, TransmissionsOperationClaims.Read),
        (24, TransmissionsOperationClaims.Update),
        (25, TransmissionsOperationClaims.Write),

        (26, UsersOperationClaims.Add),
        (27, UsersOperationClaims.Admin),
        (28, UsersOperationClaims.Delete),
        (29, UsersOperationClaims.Read),
        (30, UsersOperationClaims.Update),
        (31, UsersOperationClaims.Write),
    ];

    private static IEnumerable<OperationClaim> Seeds
        => s_claims.Select(claim => new OperationClaim { Id = claim.Id, Name = claim.Name });
}
