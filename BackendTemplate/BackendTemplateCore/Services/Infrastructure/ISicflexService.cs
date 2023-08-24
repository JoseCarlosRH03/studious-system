namespace BackendTemplateCore.Services.Infrastructure;

public interface ISicflexService
{
    Task<(SicflexResponse, string, DateTime)> RegisterInputs(string[]? args = null, DateTime? date = null);
    Task<string> PreviewInputs(string[]? args = null, DateTime? date = null);
}

public class SicflexInsertRequest
{
    public string                    fecha              { get; set; }
    public SicflexDocument           document           { get; set; }
    public int                       numero             { get; set; }
    public string                    procedencia        { get; set; }
    public int                       customerSupplierId { get; set; }
    // public DateTime                  originalDate       { get; set; }
    public string                    concepto           { get; set; }
    public SicflexCurrencyDefinition currencyDefinition { get; set; }
    public decimal                   currencyRate       { get; set; }
    public decimal                   valor              { get; set; }
    public SicflexCgadcs[]           cgadcs             { get; set; }
}

public class SicflexDocument
{
    public string id           { get; set; }
    // public string documentName { get; set; }
}

public class SicflexCurrencyDefinition
{
    public string currencyCode { get; set; }
    // public string name         { get; set; }
}

public class SicflexCgadcs
{
    public int noLinea            { get; set; }
    public SicflexCgacc cgacc     { get; set; }
    public string detalle         { get; set; }
    // public SicflexProject project { get; set; }
    
    public decimal debito         { get; set; }
    public decimal credito        { get; set; }
}

public class SicflexCgacc
{
    public string numeroCuenta { get; set; }
    // public string descripcion  { get; set; }
}
public class SicflexResponse
{
    public SicflexInsertRequest data { get; set; }
    public int status { get; set; }
    public string reasonPhrase { get; set; }
    public string message { get; set; }
}


public class SicflexProject
{
    public string id          { get; set; }
    public string description { get; set; }
}