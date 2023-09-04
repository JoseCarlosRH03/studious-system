
namespace FleetTechCore.Models.Address;

public class Country : AuditableEntity
{
    public string Name { get; set; }
    public string Demonym { get; set; }
    public int Status { get; set; }

    public virtual ICollection<State> States { get; set; }
}