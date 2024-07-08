using Core.PersistenceLayer.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.DomainLayer.Entities;

public class Brand : Entity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Model> Models { get; set; }
    public Brand()
    {
        Models = new HashSet<Model>();
    }
    public Brand(Guid id, string name, string description):this()
    {
        Id = id;
        Name = name;
        Description = description;
    }   
}
