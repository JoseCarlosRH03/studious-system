namespace FleetTechCore.Models.User;

public class Permission
{
    public int Id { get; set; }
    public int PermissionAreaId { get; set; }
    public int PermissionTypeId { get; set; }
    
    public virtual PermissionArea PermissionArea { get; set; }
    public virtual PermissionType PermissionType { get; set; }
}