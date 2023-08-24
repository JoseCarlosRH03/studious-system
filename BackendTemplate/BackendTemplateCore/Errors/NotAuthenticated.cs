namespace BackendTemplateCore.Errors;

public class NotAuthenticated : Error {
   public NotAuthenticated(string? message = null) : base(message ?? "No está autenticado.") { }
}
