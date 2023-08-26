namespace BackendTemplateCore.Models.Extensions;

public class Extension : AuditableEntity
{
    public string Name { get; set; }
    public int Status { get; set; }

    public virtual ICollection<ExtensionProperty> Properties { get; set; }
}