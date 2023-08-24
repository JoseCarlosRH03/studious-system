namespace BackendTemplateCore.Models;

public abstract class AuditableEntity {
	public int       Id             { get; set; }
	public Guid      CreatedBy      { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
	public DateTime  CreatedOn      { get; set; } = DateTime.Now;
	public Guid?     LastModifiedBy { get; set; }
	public DateTime? LastModifiedOn { get; set; }
}