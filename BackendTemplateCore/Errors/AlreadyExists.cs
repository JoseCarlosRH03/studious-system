namespace FleetTechCore.Errors;

public class AlreadyExists : Error {
   public AlreadyExists(string? message = null) : base(message ?? "Ya existe.") { }
}
