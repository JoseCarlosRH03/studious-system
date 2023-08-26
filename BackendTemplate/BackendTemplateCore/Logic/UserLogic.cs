using BackendTemplateCore.DTOs.Data;
using BackendTemplateCore.DTOs.Views;
using BackendTemplateCore.Enums;
using BackendTemplateCore.Errors;
using BackendTemplateCore.Models.User;

namespace BackendTemplateCore.Logic;

public partial class Logic
{
    public async Task<(UserView user, string token)> Login(string login_user, string password) {
        var user = await Data.GetUserLogin(login_user);
        if (user is null)
            throw new NotAuthenticated("Credenciales inválidas.");
        if (user.LockoutEnd is not null && user.LockoutEnd < DateTime.Now)
            throw new NotAuthenticated("Usuario bloqueado.");
        if (!Authentication.VerifyPassword(user, password))
            throw new NotAuthenticated("Credenciales inválidas.");
        
        user.DateLastLogin = DateTime.Now;
        Audit.SetCurrentUser(user);
        await Data.Update(user);
        
        var token = Authentication.GenerateToken(user);
        await Data.RegisterLogin(user.Id);
        
        var profilePicture = string.Empty;
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            try
            {
                profilePicture = await Resources.LoadAsDataUrl("UsersProfilePictures", user.ProfilePicture);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        return (UserView.From(user, profilePicture), token);
    }

    public async Task<User> Authenticate(string? token) {
        if (string.IsNullOrEmpty(token))
            throw new NotAuthenticated("Debe iniciar sesión.");
        var (success, user_id, date_issued) = Authentication.Authenticate(token);
        if (!success || user_id is null)
            throw new NotAuthenticated("Debe iniciar sesión.");
        var user = await Data.GetUserWithRoles(user_id.Value);
        if (user is null)
            throw new NotAuthenticated("Token inválido.");
        if (user.DateLastPasswordChange > date_issued)
            throw new NotAuthenticated("Sesión ha sido invalidada por un cambio de contraseña.");
        if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
            throw new NotAuthorized("Usuario bloqueado.");
        return user;
    }
    
    public async Task ChangePassword(Guid userId, string password) {
        if (string.IsNullOrEmpty(password.ToLower().Trim()))
            throw new InvalidParameter("Contraseña inválida.");
        var user = await Data.GetByIdAsync<User>(userId);
        if (user is null)
            throw new NotFound("Usuario no encontrado.");
        user.PasswordHash = Authentication.HashPassword(user, password);
        user.DateLastPasswordChange = DateTime.Now;
        
        await Data.Update(user);
    }
    public async Task<Guid> LockoutUser(Guid userId, int days = 0, int hours = 0) {
        var user = await Data.GetByIdAsync<User>(userId);
        if (user is null)
            throw new NotFound("Usuario no encontrado.");
        var result = await Data.LockoutUser(user, days, hours);
        return result;
    }
    public async Task<Guid> UnlockoutUser(Guid userId) {
        var user = await Data.GetByIdAsync<User>(userId);
        if (user is null)
            throw new NotFound("Usuario no encontrado.");
        var result = await Data.UnlockoutUser(user);
        return result;
    }
    public async Task<Guid> UpdateUser(Guid id, UserUpdateData data, string? origin)
    {
        Validation.ValidateUserUpdateData(data);
        var user = await Data.GetByIdAsync<User>(id);
        if (user == null) 
            throw new NotFound("No se encontro ningun usuario con ese id");
        if (data.Email.ToLower().Trim() != user.Email.ToLower().Trim())
        {
            if (await Data.ExistsUserWithEmail(data.Email))
                throw new AlreadyExists("Ya existe un usuario con este email");
            user.Email = data.Email;
        }
        if (data.Phone != user.Phone && !string.IsNullOrWhiteSpace(data.Phone))
        {
            if (await Data.ExistsUserWithPhone(data.Phone))
                throw new AlreadyExists("Ya existe un usuario con este telefono");
            user.Phone = data.Phone;
        }
        user.FirstName = data.FirstName;
        user.LastName = data.LastName;
        user.ProfilePicture = await Resources.Save("UsersProfilePictures", data.ProfilePicture, "jpg");
        await Data.Update(user);
        return user.Id;
    }
    
    public async Task<Guid> UpdateUserProfilePicture(Guid id, UserUpdatePictureData data)
    {
        var user = await Data.GetByIdAsync<User>(id);
        if (user == null) 
            throw new NotFound("No se encontro ningun usuario con ese id");
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture))
            try { await Resources.Remove("UsersProfilePictures", user.ProfilePicture); }
            catch (Exception e) { Console.WriteLine(e); }
        user.ProfilePicture = await Resources.Save("UsersProfilePictures", Convert.FromBase64String(data.ProfilePicture), data.MimeType);
        await Data.Update(user);
        return user.Id;
    }
    
    public async Task<int> CreateRole(RoleData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
            throw new InvalidParameter("El nombre no puede estar vacío");
        if (await Data.ExistsRoleWithName(data.Name))
            throw new AlreadyExists("Ya existe un rol con este nombre");
        var role = new Role
        {
            Name = data.Name,
        };
        await Data.Atomic(async () =>
        {
            role = await Data.Add(role);
            if (data.PermissionIds is not null && data.PermissionIds.Any())
            {
                var permissions = await Data.GetAll<Permission>(p => data.PermissionIds.Contains(p.Id));
                if (permissions is not null && permissions.Any())
                {
                    var rolePermissions = permissions.Select(p => new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = p.Id
                    });
                    await Data.AddRange(rolePermissions);
                }
            }
        });
        return role.Id;
    }
    public async Task<int> DeleteRole(int Id)
    {
        var role = await Data.GetAsync<Role>(r => r.Id == Id, r => r.RolePermissions);
        var userRoles = await Data.GetAll<UserRole>(ur => ur.RoleId == Id);
        if (role == null) throw new NotFound("No se encontro ningun rol con ese id");
        if (userRoles is not null && userRoles.Any()) await Data.DeleteRange(userRoles); 
        if (role.RolePermissions is not null && role.RolePermissions.Any()) await Data.DeleteRange(role.RolePermissions);
        await Data.Delete(role);
        return role.Id;
    } 
    public async Task<int> UpdateRole(int Id, RoleData data)
    {
        if (string.IsNullOrWhiteSpace(data.Name))
            throw new InvalidParameter("El nombre no puede estar vacío");
        var role = await Data.GetAsync<Role>(r => r.Id == Id, r => r.RolePermissions);
        if (role == null) throw new NotFound("No se encontro ningun rol con ese id");
        
        if (data.Name != role.Name && await Data.ExistsRoleWithName(data.Name))
            throw new AlreadyExists("Ya existe un rol con este nombre");
        role.Name = data.Name;
        // Data.Atomic adding/removing role permissions based on the data.Permissions
        
        await Data.Atomic(async () =>
        {
            if (data.PermissionIds is not null && data.PermissionIds.Any())
            {
                var permissionsToAdd = data.PermissionIds.Where(p => role.RolePermissions.All(rp => rp.PermissionId != p)).ToList();
                var permissionsToRemove = role.RolePermissions.Where(rp => !data.PermissionIds.Contains(rp.PermissionId)).ToList();
                if (permissionsToAdd.Any())
                {
                    var permissions = await Data.GetAll<Permission>(p => permissionsToAdd.Contains(p.Id));
                    if (permissions is not null && permissions.Any())
                    {
                        var rolePermissions = permissions.Select(p => new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = p.Id
                        });
                        await Data.AddRange(rolePermissions);
                    }
                }
                if (permissionsToRemove.Any())
                    await Data.DeleteRange(permissionsToRemove);
            }
            await Data.Update(role);
        });
        return role.Id;
    }
    public async Task<int> UpdateRolePermissions(int Id, List<int> Permissions)
    {
        var role = await Data.GetByIdAsync<Role>(Id);
        if (role == null) throw new NotFound("No se encontro ningun rol con ese id");
        var permissions = await Data.GetAll<RolePermission>(x => x.RoleId == Id && !Permissions.Contains(x.PermissionId));
        if (permissions is {Count: > 0})
            await Data.DeleteRange(permissions);
        await Data.AddRange(Permissions.Select(p => new RolePermission
        {
            PermissionId = p,
            RoleId = role.Id
        }));
        return role.Id;
    }
    public async Task<Guid> UpdateUserRoles(Guid Id, UserRoleData data)
    {
        var Roles = data.RoleIds;
        if (Roles is null || !Roles.Any()) throw new InvalidParameter("Debe seleccionar al menos un rol");
        if (Roles.Distinct().Count() != Roles.Count) throw new InvalidParameter("No se pueden repetir roles");
        var user = await Data.GetByIdAsync<User>(Id);
        if (user == null) throw new NotFound("No se encontro ningun usuario con ese id");
        var roles = await Data.GetAll<UserRole>(x => x.UserId == Id);
        if (roles is {Count: > 0})
            await Data.DeleteRange(roles);
        await Data.AddRange(Roles.Select(r => new UserRole
        {
            RoleId = r,
            UserId = user.Id
        }));
        if (data.BranchId is not null)
        {
            user.BranchId = data.BranchId;
            await Data.Update(user);
        }
        return user.Id;
    }
    public async Task<List<PermissionView>> GetAllPermissions() => (await Data.GetAll<Permission>(null, p => p.PermissionArea, p => p.PermissionType))?.Select(PermissionView.From).ToList() ?? throw new NotFound("No se encontro ningun permiso");
    
    public async Task<List<RoleView>> GetRoles(int start, int count, string? filter) =>
        await Data.GetRoleViews(start, count, filter) ?? throw new NotFound("No se encontro ningun rol con ese criterio de busqueda");
    
    public async Task<int> GetRoleCount(string? filter) =>
        await Data.Count<Role>(string.IsNullOrWhiteSpace(filter)? null: r => r.Name.ToLower().Contains(filter.ToLower().Trim()));

    public async Task<List<UserView>> GetUsers(int start, int count, string? filter) =>
        (await Data.GetUserViews(filter, start, count)).Select(u => UserView.From(u, null)).ToList() ?? throw new NotFound("No se encontro ningun usuario con ese criterio de busqueda");
    public async Task<int> GetUserCount(string? filter) =>
        await Data.Count<User>(string.IsNullOrWhiteSpace(filter)? null: u => u.Username.ToLower().Contains(filter.ToLower().Trim()) || (u.FirstName + " " + u.LastName).ToLower().Contains(filter.ToLower().Trim()));

    public async Task<Guid> CreateUser(UserData data)
    {
        Validation.ValidateUserData(data);
        if (await Data.GetAsync<User>(u => u.Username == data.Username) != null)
            throw new AlreadyExists("Ya existe un usuario con ese nombre de usuario");
        var result = await Data.Add(new User
        {
            FirstName = data.FirstName,
            LastName = data.LastName,
            Status = (int) GenericStatus.Activo,
            Username = data.Username,
            Email = data.Email,
            PasswordHash = Authentication.HashPassword(null, data.Password),
            Phone = data.Phone,
            BranchId = data.BranchId,
        });
        return result.Id;
    }
    
    public async Task<Guid> DeactivateUser(Guid Id)
    {
        var user = await Data.GetByIdAsync<User>(Id);
        if (user == null) throw new NotFound("No se encontro ningun usuario con ese id");
        user.LockoutEnd = DateTime.Now.AddYears(100);
        user.Status = (int)GenericStatus.Inactivo;
        await Data.Update(user);
        return user.Id;
    }
    
    public async Task<Guid> ActivateUser(Guid Id)
    {
        var user = await Data.GetByIdAsync<User>(Id);
        if (user == null) throw new NotFound("No se encontro ningun usuario con ese id");
        user.LockoutEnd = null;
        user.Status = (int)GenericStatus.Activo;
        await Data.Update(user);
        return user.Id;
    }
    
}