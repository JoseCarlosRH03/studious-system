using System.Text.Json.Serialization;

namespace BackendTemplateCore.Services.Infrastructure;

public interface IMunicipiaService
{
   Task<MunicipiaLoginResponse> Login(string user, string password);
    Task<(MunicipiaRegisteredInputResponse, string, DateTime)> RegisterInputs(string[]? args = null, DateTime? date = null);
    Task<string> PreviewInputs(string[]? args = null, DateTime? date = null);
}

public class MunicipiaLoginResponse
{
    public string            message      { get; set; }
    public MunicipiaUserData user         { get; set; }
    public string            access_token { get; set; }
}

public class MunicipiaLoginRequest
{
    public string email { get; set; }
    public string clave { get; set; }
}

public class MunicipiaUserData
{
    public string origen { get; set; }
    public string nombre { get; set; }
    public string nombreMunicipio { get; set; }
}
public class MunicipiaLegacyUserData
{
    [JsonPropertyName("0")]
    public MunicipiaLegacyUser user { get; set; }
    public string        logo { get; set; }
}

public class MunicipiaLegacyUser
{
    public string logo             { get; set; }
    public string cod              { get; set; }
    public string username         { get; set; }
    public string email            { get; set; }
    public string codigoMunicipio  { get; set; }
    public string nombreMunicipio  { get; set; }
}

public class MunicipiaRegisteredInputRequest
{
    public string                            apikey        { get; set; }
    public string                            origen        { get; set; }
    public string                            fecha         { get; set; }
    public string                            efectivo      { get; set; }
    public string                            cheques       { get; set; }
    public string                            tarjeta       { get; set; }
    public string                            transferencia { get; set; }
    public string                            otros         { get; set; }
    public MunicipiaRegisteredInputDetails[] detalle       { get; set; }
}

public class MunicipiaRegisteredInputDetails
{
    public string clasificador { get; set; }
    public string monto { get; set; }
}

public class MunicipiaRegisteredInputResponse
{
    public string message { get; set; }
    public int dato    { get; set; }
    public bool   success { get; set; }
}