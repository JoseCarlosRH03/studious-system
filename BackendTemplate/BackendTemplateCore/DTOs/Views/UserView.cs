using BackendTemplateCore.DTOs.Shared;
using BackendTemplateCore.Enums;
using BackendTemplateCore.Models.User;

namespace BackendTemplateCore.DTOs.Views;

public record struct UserView
(
    Guid Id,
    string Name,
    string FirstName,
    string LastName,
    string? ProfilePicture,
    string UserName,
    string Email,
    string? Phone,
    bool IsActive,
    BranchView? Branch,
    Item[]? Roles,
    PermissionView[] Permissions,
    Item Status
){
    public static UserView From(User user, string? ProfilePicture)
    {
        
        var permissions = user.Roles is null || !user.Roles.Any()? new List<PermissionView>() :
            user.Roles.SelectMany(x =>  x.Role.RolePermissions).Select(y => PermissionView.From(y.Permission)).ToList();
        permissions.AddRange( user.Claims is null || !user.Claims.Any()? new List<PermissionView>() :
            user.Claims.Select(x => PermissionView.From(x.Permission)).ToList());
        permissions = permissions.DistinctBy(x => x.Description).ToList();

        return new UserView
        {
            Id = user.Id,
            Name = user.FirstName + " " + user.LastName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = ProfilePicture,
            UserName = user.Username,
            Email = user.Email,
            Phone = user.Phone,
            IsActive = user.Status == (int) GenericStatus.Activo,
            Branch = user.Branch is not null ? BranchView.From(user.Branch) : null,
            Roles = user.Roles?.Select(x => new Item(x.RoleId, x.Role.Name)).ToArray(),
            Permissions = permissions.ToArray(),
            Status = Item.From((GenericStatus) user.Status)
        };
    }
}