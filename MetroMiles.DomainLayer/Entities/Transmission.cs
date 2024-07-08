using Core.PersistenceLayer.Repositories.Entities;

namespace MetroMiles.DomainLayer.Entities;

public class Transmission : Entity<Guid>
{
    public string Name { get; set; }
    public virtual ICollection<Model> Models { get; set; }
    public Transmission()
    {
        Models = new HashSet<Model>();
    }

    public Transmission(Guid id, string name) : base(id)
    {
        Id = id;
        Name = name;
    }
}
