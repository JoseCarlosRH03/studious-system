using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Errors;
using FleetTechCore.Models.Company;
using FleetTechCore.Models.Fleet;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace FleetTechCore.Logic;

public partial class Logic
{

    public async Task<List<Driver>> GetAllDrivers() => (await Data.GetAll<Driver>()).ToList() ?? throw new NotFound("No se encontro ningun conductor");
    public async Task<List<VehicleView>> GetAllVehicle() => (await Data.GetAll<Vehicle>(null, x => x.FuelType))
                 .Select(v => VehicleView.From(v))
                 .ToList()
                 ?? throw new NotFound("No se encontro ningun conductor");
    public Task<List<Item>> GetAllVehicleState() => (GetVehicleState()) ?? throw new NotFound("No se encontro ningun estado");
    public Task<List<Item>> GetAllVehicleType() => (GetVehicleType()) ?? throw new NotFound("No se encontro ningun tipo");
    public async Task<List<Item>> GetAllLicenseType() => (await Data.GetAll<LicenseType>())
        .Select(l => new Item(l.Id, l.Description))
        .ToList() ?? throw new NotFound("No se encontro ningun tipo de licencia");

    public async Task<VehicleView> GetVehicleById(int Id) {

        var result = await Data.GetVehicleById(Id);

        if(result is null)
             throw new NotFound("No se encontro ningun tipo");

        return VehicleView.From(result);
    }
    public async Task<int> CreateVehicle(VehicleData data)
    {
       Validation.ValidateVehicleData(data);

        if (await Data.GetAsync<Vehicle>(v => v.Chassis == data.Chassis || v.Code == data.Code || v.LicensePlate == data.LicensePlate) is not null)
            throw new AlreadyExists("Ya exsite un vehiculo con alguno de los datos suministrados");

        var result = await Data.Add<Vehicle>( new Vehicle {
             Code              = data.Code,                    
             PolicyDescription = data.PolicyDescription,       
             PolicyNumber      = data.PolicyNumber,            
             PolicyReference   = data.PolicyReference,         
             PolicyExpiration  = data.PolicyExpiration,        
             Status            = data.Status,         
             Type              = data.Type,                    
             Brand             = data.Brand,                   
             Model             = data.Model,                   
             Year              = data.Year,                   
             LicensePlate      = data.LicensePlate,            
             Color             = data.Color,                   
             FuelTypeId        = data.FuelTypeId,                
             FuelCapacity      = data.FuelCapacity,            
             FuelPerMonth      = data.FuelPerMonth,            
             Mileage           = data.Mileage,                 
             Chassis           = data.Chassis,
             Engine            = data.Engine,
        });
           
        return result.Id;
    }


}