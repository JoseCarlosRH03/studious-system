using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Shared;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Fleet;
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
        if(await Data.ExistsStationWithRnc(data.RNC))  throw new AlreadyExists("Ya existe una estaci�n con este RNC");

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
        var station = await Data.GetFuelStationById(data.Id);

        if (station == null) throw new NotFound("No se encontro estacion de combustible");
        await Data.Atomic(async () => {

            station.Code = data.Code;
            station.CompanyName = data.CompanyName;
            station.RNC = data.RNC;
            station.Phone = data.Phone;
            station.Email = data.Email;
            station.Status = data.StatusId;

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
    public async Task<int> DeleteFuelSatio(int Id, User user)
    {
        var station = await Data.GetFuelStationById(Id);

        if (station == null) throw new NotFound("No se encontro estacion de combustible");
        station.Status = (int)GenericStatus.Inactivo;
        await Data.Update(station, user.Id);

        return station.Id;
    }

    public async Task<List<PriceView>> GetAllPrice() => (await Data.GetAllFuelPrice());
    public async Task<PriceView> GetFuelPriceById(int Id)
    {
        var price = await Data.GetFuelPriceById(Id);
        if (price == null) throw new NotFound("No se encontro ningun precio de combustible");

        return PriceView.From(price);
    }

    public async Task<int> CreateFuelPrice(FuelPriceData data, User user)
    {
        Validation.ValidateFuelPriceData(data);
        try
        {
            var result = await Data.Add<FuelPrice>(new FuelPrice
            {
                FuelTypeId = data.FuelTypeId,
                DateFrom = data.DateFrom,
                DateTo = data.DateTo,
                Price = data.price,
                Status = (int)GenericStatus.Activo
            });

            return result.Id;
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    public async Task<int> UpdateFuelPrice(int id, FuelPriceData data, User user)
    {
        Validation.ValidateFuelPriceData(data);
        var price = await Data.GetByIdAsync<FuelPrice>(id);

        await Data.Atomic(async () =>
        {
            if (price == null) throw new NotFound("No existe este precio de combustible");
            price.FuelTypeId = data.FuelTypeId;
            price.DateFrom = data.DateFrom;
            price.DateTo = data.DateTo;
            price.Price = data.price;
            price.Status = (int)GenericStatus.Activo;
            await Data.Update<FuelPrice>(price);
        });

        return price.Id;
    }

    public async Task<int> InactiveFuelPrice(int id, User user)
    {
        var price = await Data.GetByIdAsync<FuelPrice>(id);
        await Data.Atomic(async () =>
        {
            if (price == null) throw new NotFound("No existe este precio de combustible");
            price.Status = (int)GenericStatus.Inactivo;
            await Data.Update<FuelPrice>(price, user.Id);
        });

        return price.Id;
    }
}