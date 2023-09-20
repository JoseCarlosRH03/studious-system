using System.Reflection;
using System.Text.RegularExpressions;
using FleetTechCore.DTOs.Data;
using FleetTechCore.Errors;
using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FleetTechCore;

public class Validation
{
    static void Validate<T>(T data, string parameter_name, Func<T, bool> validator, string error_message) {
        if (!validator(data))
            throw new InvalidParameter($"Parámetro inválido: {parameter_name}\n - {error_message}");
    }

    static bool IsNull<T>(T data) =>
        data is null
        || (data is string  && string.IsNullOrWhiteSpace(data as string))
        || (data is int     && (data as int?) == 0)
        || (data is float   && (data as float?) == 0)
        || (data is double  && (data as double?) == 0)
        || (data is decimal && (data as decimal?) == 0);

    static void Require<T>(T data, string parameter_name) {
        if (IsNull(data))
            throw new InvalidParameter($"Parámetro inválido: {parameter_name}\n - Requerido.");
    }
    static Func<T, bool> IsNullOr<T>(Func<T, bool> check) => (T data) => IsNull(data) || check(data);

    static readonly Regex DocumentFormat    = new(@"^\s*\d{3}-?\d{7}-?\d\s*$");
    static readonly Regex PhoneFormat       = new(@"^\s*\(?\d{3}\)?[-\s]?\d{3}[-\s]?\d{4}\s*$");
    static readonly Regex EmailFormat       = new(@"^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");
    
    public static void ValidateEmail(string email) {
        Require(email, "Correo electrónico");
        Validate(email, "Correo electrónico", EmailFormat.IsMatch, "Correo inválido.");
    }
    public static void ValidateUserData(UserData data) {
        Require(data.FirstName, "Nombres");
        Require(data.LastName, "Apellidos");
        Require(data.Password, "Contraseña");
        Validate(data, "Datos de acceso", _ => _.Email?.NullIfNothing() is not null, "Es necesario que el usuario tenga un correo" );
        Validate(data.Password, "Contraseña", _ => !(_.StartsWith(" ") || _.EndsWith(" ")), "No puede empezar ni terminar con espacios.");
        Validate(data.Password, "Contraseña", _ => _.Length >= 8, "Debe tener al menos 8 caracteres.");
        Validate(data.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "Correo inválido.");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }
    public static void ValidateUserUpdateData(UserUpdateData data)
    {
        Require(data.FirstName, "Nombres");
        Require(data.LastName, "Apellidos");
        Validate(data, "Datos de acceso", _ => _.Email?.NullIfNothing() is not null || _.Phone?.NullIfNothing() is not null, "Es necesario registrarse con correo o numero de celular" );
        Validate(data.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "Correo inválido.");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCityData(CityData data)
    {
        Require(data.Name, "Nombre");
        Require(data.StateId, "Provincia/Estado");
    }

    public static void ValidateStateData(StateData data)
    {
        Require(data.Name, "Nombre");
        Require(data.CountryId, "Provincia/Estado");
    }
    public static void ValidateBranchData(BranchData data)
    {
        Require(data.Code, "Código");
        Require(data.CityId, "Id de ciudad");
        Require(data.BranchTypeId, "Tipo de sucursal");
        Require(data.Locality, "Localidad");
        Require(data.Address, "Dirección");
        Require(data.Phone, "Teléfono");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCompanyData(CompanyData data)
    {
        Require(data.Name, "Nombre");
        Require(data.TaxRegistrationNumber, "RNC");
        Require(data.AddressLine1, "Dirección");
        Require(data.AddressLine2, "Dirección");
        Require(data.CityId, "Ciudad");
        Require(data.Phone, "Teléfono");
        ValidateEmail(data.Email);
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCompanySettingsData(CompanySettingsData data)
    {
        Require(data.TimeZone, "Zona horaria");
        Require(data.DatePattern, "Formato de fecha");
        Require(data.TimePattern, "Formato de hora");
        Require(data.LateFeeProductId, "Producto de mora");
    }

    public static void ValidateVehicleData(VehicleData data)
    {
        Require(data.PolicyNumber, "Numero de poliza");
        Require(data.PolicyReference, "Referencia de poliza");
        Require(data.PolicyExpiration, "Expiración de poliza");
        Require(data.Status, "Estado");
        Require(data.Type, "Tipo");
        Require(data.Brand, "Marca");
        Require(data.Model, "Modelo");
        Require(data.Year, "Año");
        Require(data.LicensePlate, "Matrícula");
        Require(data.Color, "Color");
        Require(data.FuelTypeId, "Tipo de combustible");
        Require(data.FuelCapacity, "Cap. de combustible");
        Require(data.FuelPerMonth, "Combustible al mes");
        Require(data.Mileage, "Kilometraje");
        Require(data.Chassis, "Chasis");
        Require(data.Engine, "Motor");
    }

    public static void ValidateDriverData(DriverData data)
    {
        Require(data.EmployeeCode, "Código del empleado");
        Require(data.FirstName, "Nombre del conductor");
        Require(data.LastName, "Apellido del conductor");
        Require(data.IdentityDocument, "Documento de identidad");
        Require(data.DateOfBirth, "Fecha de nacimiento");
        Require(data.ExpirationOfTheLicense, "Expiración de la licencia");
        Require(data.LicenseCategory_id, "Categoría de la licencia");
        Require(data.License_id, "Licencia");
        Require(data.LicenseFileName, "Documento de la licencia");
        Require(data.Phone, "Telefono del conductor");
        Require(data.DateOfHire, "Fecha de contratación");
    }

    public static void ValidateDriverData(StationData data)
    {
        Require(data.Code, "Código de la estación");
        Require(data.CompanyName, "Nombre de la  compañia");
        Require(data.RNC, "RNC");
        Require(data.Phone, "Telefono");
        Require(data.Email, "Correo");
        Require(data.Address, "Dirección");      
    }
}