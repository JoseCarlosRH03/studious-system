using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Supply;
using FleetTechCore.Models.User;
using FleetTechCore.Models.WorkShop;

namespace FleetTechCore.Logic;

public partial class Logic
{
    public async Task<List<MechanicalWorkshopView>> GetAllMechanicalWorkshop() => (await Data.GetAllMechanicalWorkshop());
    public async Task<MechanicalWorkshopView> GetMechanicalWorkshopById(int Id) {
        var supply = await Data.GetMechanicalWorkshopById(Id);

        if (supply == null) throw new NotFound("No se encontro suplidor");

        return MechanicalWorkshopView.From(supply);
    }

    public async Task<int> CreateMechanicalWorkshop(MechanicalWorkshopData data, User user)
    {
        // confirmar validaciones 
        if(await Data.ExistsMechanicalWorkshopWithRnc(data.RNC))  throw new AlreadyExists("Ya existe una suplidor con este RNC");

        MechanicalWorkshop supply = new MechanicalWorkshop
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

            supply.AddressId = address.Id;
            await Data.Add( supply);

                var concats = data.Contacts.Select(c => new Contact { Name = c.Name , Telephone =c.Phone , Email = c.Email, MechanicalWorkshopId = supply.Id });

                await Data.AddRange(concats);
            
       }); 

        return supply.Id;                                                 
    }

    public async Task<int> UpdateMechanicalWorkshop(MechanicalWorkshopData data, User user)
    {
        var supply = await Data.GetMechanicalWorkshopById(data.Id);

        if (supply == null) throw new NotFound("No se encontro suplidor");
        await Data.Atomic(async () => {

            supply.Code = data.Code;
            supply.CompanyName = data.CompanyName;
            supply.RNC = data.RNC;
            supply.Phone = data.Phone;
            supply.Email = data.Email;
            supply.Status = data.StatusId;
            await Data.Update( supply,user.Id);

            supply.Address.PlainAddress = $"{data.AddressLine1} {data.AddressLine2} {data.AddressLine3}";
            supply.Address.AddressLine1 = data.AddressLine1;
            supply.Address.AddressLine2 = data.AddressLine2;
            supply.Address.AddressLine3 = data.AddressLine3;
            supply.Address.CityId = data.CityId;

            await Data.Update(supply.Address, user.Id);

            if(data.Contacts.Count == 0)
            {
              await Data.DeleteRange(supply.Contacts);
            }
            else
            {
                var toConctact = data.Contacts.Select(c => new Contact { Name = c.Name, Telephone = c.Phone, Email = c.Email, MechanicalWorkshopId = supply.Id });
                var newContact = toConctact.Where(c => c.Id == 0).ToList();

                var delete = supply.Contacts.Except(toConctact).ToList();
                if (delete?.Count > 0) await Data.DeleteRange<Contact>(delete);

                if (newContact.Count > 0) await Data.AddRange<Contact>(newContact);

                var updateContact = toConctact.Intersect(supply.Contacts).ToList();
                if (updateContact?.Count > 0) 
                {
                    foreach (var contact in updateContact)
                    {
                        await Data.Update<Contact>(contact);
                    }
                }
            }

        });

        return supply.Id;
    }
    public async Task<int> DeleteMechanicalWorkshop(int Id, User user)
    {
        var supply = await Data.GetMechanicalWorkshopById(Id);

        if (supply == null) throw new NotFound("No se encontro estacion de combustible");
        supply.Status = (int)GenericStatus.Inactivo;
        await Data.Update(supply, user.Id);

        return supply.Id;
    }
}