using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Errors;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Fuel;
using FleetTechCore.Models.User;

namespace FleetTechCore.Logic;

public partial class Logic
{

    public async Task<List<Item>> GetAllFuelType() => (await Data.GetAll<FuelType>())
        .Select(f => new Item(f.Id,f.Name)).ToList() ?? throw new NotFound("No se encontro ningun tipo de combustible");

    public async Task<List<ServicePlaseView>> GetAllFuelSation() => (await Data.GetAllFuelStation()); 

    public async Task<int> CreateStation(ServicePlaseData data, User user)
    {
        if(await Data.ExistsStationWithRnc(data.RNC))  throw new AlreadyExists("Ya existe una estación con este RNC");

        FuelStation station = new FuelStation
        {
            Code = data.Code,
            CompanyName = data.CompanyName,
            RNC = data.RNC,
            Phone = data.Phone,
            Email = data.Email,

        };

       await Data.Atomic( async () => { 
            var address = await Data.Add(new Address
                {
                    PlainAddress = $"{data.AddressLine1} {data.AddressLine2} {data.AddressLine3}" ,
                    AddressLine1 = data.AddressLine1,
                    AddressLine2 = data.AddressLine2,
                    AddressLine3 = data.AddressLine3,
                    CityId = data.CityId,
            });

            station.AddressId = address.Id;
            await Data.Add( station);

                var concats = data.Contacts.Select(c => new Contact { Name = c.Name , Telephone =c.Phone , Email = c.Email, FuelStationId = station.Id });

                await Data.AddRange(concats);
            
       }); 

        return station.Id;                                                 
    }                                                         
   
}