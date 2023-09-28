using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Supply;
using FleetTechCore.Models.User;

namespace FleetTechCore.Logic;

public partial class Logic
{
    public async Task<List<SupplyView>> GetAllSupply() => (await Data.GetAllSupply());
    public async Task<SupplyView> GetSupplyById(int Id) {
        var supply = await Data.GetSupplyById(Id);

        if (supply == null) throw new NotFound("No se encontro suplidor");

        return SupplyView.From(supply);
    }

    public async Task<int> CreateSupply(SupplyData data, User user)
    {
        // confirmar validaciones 
        if(await Data.ExistsSuplyWithRnc(data.RNC))  throw new AlreadyExists("Ya existe una suplidor con este RNC");

        Supplier supply = new Supplier
        {
            Code = data.Code,
            Name = data.Name,
            Position = data.Position,
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

            supply.AddressId = address.Id;
            await Data.Add( supply);

            var concats = data.Contacts.Select(c => new Contact { Name = c.Name , Telephone =c.Phone , Email = c.Email, SupplierId = supply.Id });

            await Data.AddRange(concats);
            
       }); 

        return supply.Id;                                                 
    }

    public async Task<int> UpdateSupply(SupplyData data, User user)
    {
        var supply = await Data.GetSupplyById(data.Id);

        if (supply == null) throw new NotFound("No se encontro suplidor");
        await Data.Atomic(async () => {

            supply.Code = data.Code;
            supply.CompanyName = data.CompanyName;
            supply.RNC = data.RNC;
            supply.Phone = data.Phone;
            supply.Email = data.Email;
            supply.Status = data.StatusId;
            supply.Name = data.Name;
            supply.Position = data.Position;
            await Data.Update( supply,user.Id);

            supply.Address.PlainAddress = $"{data.AddressLine1} {data.AddressLine2} {data.AddressLine3}";
            supply.Address.AddressLine1 = data.AddressLine1;
            supply.Address.AddressLine2 = data.AddressLine2;
            supply.Address.AddressLine3 = data.AddressLine3;
            supply.Address.CityId = data.CityId;

            await Data.Update(supply.Address, user.Id);

            await ManagemmentContact(data.Contacts, supply.Id, "supply", supply.Contacts);

        });

        return supply.Id;
    }
    public async Task<int> DeleteSupply(int Id, User user)
    {
        var supply = await Data.GetSupplyById(Id);

        if (supply == null) throw new NotFound("No se encontro estacion de combustible");
        supply.Status = (int)GenericStatus.Inactivo;
        await Data.Update(supply, user.Id);

        return supply.Id;
    }
}