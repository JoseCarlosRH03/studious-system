namespace FleetTechCore.Errors;

public class Error: Exception {
   public string          Code    { get; protected set; }
   public override string Message { get; }

   public Error(string code, string message) { Code = code; Message = message; }
   public Error(string message) { Code = GetType().Name.PascalCaseToSnakeCase().ToUpper(); Message = message; }
   public Error(Exception ex) : this("INTERNAL_ERROR", $"[{ex.GetType().Name}] {ex.Message}") { }
}