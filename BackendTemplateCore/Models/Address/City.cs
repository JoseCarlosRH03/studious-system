
namespace FleetTechCore.Models.Address;

public class City : AuditableEntity
{
    public string Name { get; set; }
    public int StateId { get; set; }
    public int Status { get; set; }
    
    public virtual State State { get; set; }
}