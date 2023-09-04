namespace FleetTechCore.Enums;

public enum AuditTypes
{
    None = 1,
    Created,
    Updated,
    Deleted,
    Voided,
    Login,
    Lockout,
    Unlockout
}