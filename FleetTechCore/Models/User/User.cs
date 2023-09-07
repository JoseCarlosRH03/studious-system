using FleetTechCore.Models.Company;

namespace FleetTechCore.Models.User;

public class User {
	public Guid      Id                     { get; set; }
	public string    FirstName              { get; set; }
	public string    LastName               { get; set; }
	public string?   ProfilePicture         { get; set; }
	public int       Status                 { get; set; }
	public string?	 Document               { get; set; }
	public string    Username               { get; set; }
	public string    Email                  { get; set; }
	public string    PasswordHash           { get; set; }
	public string?   Phone                  { get; set; }

	public DateTime  DateCreated            { get; set; } = DateTime.Now;
	public int       AccessFailedCount      { get; set; } = 0;
	public DateTime? LockoutEnd             { get; set; }
	public DateTime?  DateLastLogin          { get; set; }
	public DateTime?  DateLastLoginFieldService { get; set; }
	public DateTime?  DateLastPasswordChange { get; set; }

	public int? BranchId { get; set; }
	public virtual Branch? Branch { get; set; }
	public virtual ICollection<UserRole>       Roles { get; set; }
	public virtual ICollection<UserPermission> Claims { get; set; }

}