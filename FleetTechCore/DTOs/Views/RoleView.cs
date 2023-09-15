using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.User;

namespace FleetTechCore.DTOs.Views;

public record struct RoleView(
    int Id,
    string Name,
    PermissionView[] Permissions
)
{
    public static RoleView From(Role role) => new(
        role.Id,
        role.Name,
        role.RolePermissions.Select(p => PermissionView.From(p.Permission)).ToArray()
    );
}
public record struct PermissionView(
    int Id,
    string Description,
    Item Area,
    Item Type)
{
    public static PermissionView From(Permission permission) => new(
        permission.Id,
        Item.From((PermissionAreas)permission.PermissionAreaId).Description + "." + Item.From((PermissionTypes)permission.PermissionTypeId).Description,
        Item.From((PermissionAreas)permission.PermissionAreaId),
        Item.From((PermissionTypes)permission.PermissionTypeId)
    );
}