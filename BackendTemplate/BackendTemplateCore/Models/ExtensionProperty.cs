namespace BackendTemplateCore.Models;

public class ExtensionProperty : AuditableEntity
{
    public string Name { get; set; }
    public string Value { get; set; }
    public int ExtensionId { get; set; }
    
    public virtual Extension Extension { get; set; }
}