using System.Reflection;
using Core.SecurityLayer.Constants;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Extensions.ServiceRegistrations;
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

        builder.HasQueryFilter(oc => !oc.DeletedDate.HasValue);

        builder.HasMany(oc => oc.UserOperationClaims);

        builder.HasData(Seeds);
    }

    private static IEnumerable<OperationClaim> Seeds
    {
        get
        {
            var id = 0;

            yield return new OperationClaim { Id = ++id, Name = GeneralOperationClaims.Admin };

            #region Feature Operation Claims
            // Assembly.GetTypes()/Type.GetFields() order is unspecified by .NET — without an explicit
            // sort, the same code can seed different IDs on different machines/builds, silently
            // reassigning what an already-seeded ID means (e.g. "brands.admin" -> "transmissions.admin").
            // Sorting by name makes seeding deterministic and reproducible across environments.
            var featureOperationClaimsTypes = Assembly
                .GetAssembly(typeof(ApplicationServiceRegistration))!
                .GetTypes()
                .Where(
                    type =>
                        (type.Namespace?.Contains("Features") == true)
                        && (type.Namespace?.Contains("Constants") == true)
                        && type.IsClass
                        && type.Name.EndsWith("OperationClaims", StringComparison.Ordinal)
                )
                .OrderBy(type => type.FullName, StringComparer.Ordinal);
            foreach (var type in featureOperationClaimsTypes)
            {
                var typeFields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                    .OrderBy(typeField => typeField.Name, StringComparer.Ordinal);
                var typeFieldsValues = typeFields.Select(typeField => typeField.GetValue(null)!.ToString()!);

                var featureOperationClaimsToAdd = typeFieldsValues.Select(
                    value => new OperationClaim { Id = ++id, Name = value }
                );
                foreach (var featureOperationClaim in featureOperationClaimsToAdd)
                {
                    yield return featureOperationClaim;
                }
            }
            #endregion
        }
    }
}
