using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Company;
using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.User;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Linq;
using System.Reflection;
namespace FleetTechCore.Logic;

public partial class Logic
{
    //drivers

    public async Task<List<DriverView>> GetAllDrivers(int start = 0, int count = 20, string? filter = "") =>
        await Data.GetDriver(start, count, filter);

    public async Task<List<Item>> GetAllLicenseType() => (await Data.GetAll<LicenseType>())
        .Select(l => new Item(l.Id, l.Description))
        .ToList() ?? throw new NotFound("No se encontro ningun tipo de licencia");

    public async Task<DriverView> GetDriver(int id)
    {
        var driver = await Data.GetAsync<Driver>(x => x.Id == id);
        if (driver == null)
            throw new NotFound("No existe este conductor");
        return DriverView.From(driver);
    }

    public async Task<int> CreateDriver(DriverData data)
    {
        Validation.ValidateDriverData(data);
        if (await Data.GetAsync<Driver>(d => d.EmployeeCode == data.EmployeeCode) is not null)
            throw new AlreadyExists("Ya existe un conductor registrado con ese código");

        var result = await Data.Add<Driver>(new Driver
        {
            EmployeeCode = data.EmployeeCode,
            FirstName = data.FirstName,
            LastName = data.LastName,
            DateOfBirth = data.DateOfBirth,
            LicenseCategory_id = data.LicenseCategory_id,
            ExpirationOfTheLicense = data.ExpirationOfTheLicense,
            IdentityDocument = data.IdentityDocument,
            Phone = data.Phone,
            LicenseFileName = data.LicenseFileName,
            DateOfHire = data.DateOfHire,
            Status = (int)GenericStatus.Activo
        });

        return result.Id;
    }

    public async Task<int> UpdateDriver(int id, DriverData data)
    {
        Validation.ValidateDriverData(data);
        if (await Data.GetAsync<Driver>(d => d.EmployeeCode == data.EmployeeCode) is not null)
            throw new AlreadyExists("Ya existe un conductor registrado con ese código");
        var driver = await Data.GetByIdAsync<Driver>(id);
        if (driver == null)
            throw new NotFound("No existe este conductor");
        driver.FirstName = data.FirstName.Trim();
        driver.LastName = data.LastName.Trim();
        driver.DateOfBirth = data.DateOfBirth;
        driver.LicenseCategory_id = data.LicenseCategory_id;
        driver.ExpirationOfTheLicense = data.ExpirationOfTheLicense;
        driver.IdentityDocument = data.IdentityDocument;
        driver.Phone = data.Phone;
        driver.LicenseFileName = data.LicenseFileName;
        driver.DateOfHire = data.DateOfHire;
        await Data.Update(driver);
        return driver.Id;
    }

    public async Task<int> InactiveDriver(int id)
    {
        var driver = await Data.GetByIdAsync<Driver>(id);
        if (driver == null)
            throw new NotFound("No existe este conductor");
        driver.Status = (int)GenericStatus.Inactivo;
        await Data.Update(driver);
        return driver.Id;
    }



    // vehicle
    public Task<List<Item>> GetAllVehicleState() => (GetVehicleState()) ?? throw new NotFound("No se encontro ningun estado");
    public Task<List<Item>> GetAllVehicleType() => (GetVehicleType()) ?? throw new NotFound("No se encontro ningun tipo");
    public async Task<List<VehicleView>> GetAllVehicle() => (await Data.GetAll<Vehicle>(null, x => x.FuelType))
              .Select(v => VehicleView.From(v))
              .ToList()
              ?? throw new NotFound("No se encontro ningun conductor");
    public async Task<int> CreateVehicle(VehicleData data)
    {
        Validation.ValidateVehicleData(data);

        if (await Data.GetAsync<Vehicle>(v => v.Chassis == data.Chassis || v.Code == data.Code || v.LicensePlate == data.LicensePlate) is not null)
            throw new AlreadyExists("Ya existe un vehiculo con alguno de los datos suministrados");

        var result = await Data.Add<Vehicle>(new Vehicle
        {// pendiente ver que hacer con el code si llega null
            Code = data.Code,
            PolicyDescription = data.PolicyDescription,
            PolicyNumber = data.PolicyNumber,
            PolicyReference = data.PolicyReference,
            PolicyExpiration = data.PolicyExpiration,
            Status = data.Status,
            Type = data.Type,
            Brand = data.Brand,
            Model = data.Model,
            Year = data.Year,
            LicensePlate = data.LicensePlate,
            Color = data.Color,
            FuelTypeId = data.FuelTypeId,
            FuelCapacity = data.FuelCapacity,
            FuelPerMonth = data.FuelPerMonth,
            Mileage = data.Mileage,
            Chassis = data.Chassis,
            Engine = data.Engine,
        });

        return result.Id;
    }

    public async Task<VehicleView> GetVehicleById(int Id)
    {

        var result = await Data.GetVehicleById(Id);
        if (result is null)
            throw new NotFound("No se encontro ningun vehiculo");

        return VehicleView.From(result);
    }
    public async Task<int> UpdateVehicle(VehicleData data, User user)
    {
        Validation.ValidateVehicleData(data);
        var result = await Data.GetAsync<Vehicle>(v => v.Id == data.Id);
        if (result is  null)
            throw new AlreadyExists("Ya existe un vehiculo con alguno de los datos suministrados");

        result.Code = data.Code;
        result.PolicyDescription = data.PolicyDescription;
        result.PolicyNumber = data.PolicyNumber;
        result.PolicyReference = data.PolicyReference;
        result.PolicyExpiration = data.PolicyExpiration;
        result.Status = data.Status;
        result.Type = data.Type;
        result.Brand = data.Brand;
        result.Model = data.Model;
        result.Year = data.Year;
        result.LicensePlate = data.LicensePlate;
        result.Color = data.Color;
        result.FuelTypeId = data.FuelTypeId;
        result.FuelCapacity = data.FuelCapacity;
        result.FuelPerMonth = data.FuelPerMonth;
        result.Mileage = data.Mileage;
        result.Chassis = data.Chassis;
        result.Engine = data.Engine;

       await Data.Update<Vehicle>(result,user.Id);
            
        return result.Id;
    }

    public async Task<VehicleView> DeleteVehicle(int Id, User user)
    {

        var result = await Data.GetVehicleById(Id);
        if (result is null)
            throw new NotFound("No se encontro ningun vehiculo");

        result.Status = (int)VehicleState.Inactivo;
        await Data.Update<Vehicle>(result, user.Id);
        return VehicleView.From(result);
    }


}