using FleetTechCore.DTOs.Views;
using FleetTechCore.Errors;
using FleetTechCore.Models.Fleet;

namespace FleetTechCore.Logic;

public partial class Logic
{

    public async Task<List<Driver>> GetAllDrivers() => (await Data.GetAll<Driver>()).ToList() ?? throw new NotFound("No se encontro ningun conductor");
    public async Task<List<Vehicle>> GetAllVehicle() => (await Data.GetAll<Vehicle>()).ToList() ?? throw new NotFound("No se encontro ningun conductor");


}