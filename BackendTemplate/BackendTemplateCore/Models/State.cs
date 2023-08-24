
namespace BackendTemplateCore.Models;

public class State : AuditableEntity
{
    public string Name { get; set; }
    public int CountryId { get; set; }
    public int Status { get; set; }

    public virtual Country Country { get; set; }
    public virtual ICollection<City> Cities { get; set; }
}