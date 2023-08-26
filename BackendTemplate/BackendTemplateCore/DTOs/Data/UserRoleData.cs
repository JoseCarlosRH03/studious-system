namespace BackendTemplateCore.DTOs.Data;

public record struct UserRoleData(
    int? BranchId,
    List<int> RoleIds
);