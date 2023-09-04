namespace FleetTechCore.DTOs.Data;

public record struct RoleData(
    string Name,
    int[] PermissionIds
    );