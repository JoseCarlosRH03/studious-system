namespace BackendTemplateCore.Models;

public class Audit
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }

    public virtual User User { get; set; }

}