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
    public async Task<ServicePlaseView> GetFuelSationById(int Id) {
        var station = await Data.GetFuelStationById(Id);

        if (station == null) throw new NotFound("No se encontro estacion de combustible");

        return ServicePlaseView.From(station);
    }

    public async Task<int> CreateStation(ServicePlaseData data, User user)
    {
        // confirmar validaciones 
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

    public async Task<int> UpdateStation(ServicePlaseData data, User user)
    {
        // confirmar validaciones 
        if (await Data.ExistsStationWithRnc(data.RNC)) throw new AlreadyExists("Ya existe una estación con este RNC");

        var station = await Data.GetFuelStationById(data.Id);

        if (station == null) throw new NotFound("No se encontro estacion de combustible");
        await Data.Atomic(async () => {

            station.Code = data.Code;
            station.CompanyName = data.CompanyName;
            station.RNC = data.RNC;
            station.Phone = data.Phone;
            station.Email = data.Email;

            await Data.Update( station,user.Id);

            station.Address.PlainAddress = $"{data.AddressLine1} {data.AddressLine2} {data.AddressLine3}";
            station.Address.AddressLine1 = data.AddressLine1;
            station.Address.AddressLine2 = data.AddressLine2;
            station.Address.AddressLine3 = data.AddressLine3;
            station.Address.CityId = data.CityId;

            await Data.Update(station.Address, user.Id);

            if(data.Contacts.Count == 0)
            {
              await Data.DeleteRange(station.Contacts);
            }
            else
            {
                var toConctact = data.Contacts.Select(c => new Contact { Name = c.Name, Telephone = c.Phone, Email = c.Email, FuelStationId = station.Id });
                var newContact = toConctact.Where(c => c.Id == 0).ToList();

                var delete = station.Contacts.Except(toConctact).ToList();
                if (delete.Count > 0) await Data.DeleteRange<Contact>(delete);

                if (newContact.Count > 0) await Data.AddRange<Contact>(newContact);

                var updateContact = toConctact.Intersect(station.Contacts).ToList();
                if (updateContact.Count > 0) 
                {
                    foreach (var contact in updateContact)
                    {
                        await Data.Update<Contact>(contact);
                    }
                }
            }

        });

        return station.Id;
    }
}