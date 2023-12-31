﻿namespace FleetTechCore.Models.User;

public class UserPermission
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int PermissionId { get; set; }
    
    public virtual Permission Permission { get; set; }
    public virtual Models.User.User User { get; set; }
}