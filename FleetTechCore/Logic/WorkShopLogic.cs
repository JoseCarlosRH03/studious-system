using FleetTechCore.DTOs.Data;
using FleetTechCore.DTOs.Views;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models;
using FleetTechCore.Models.Address;
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

        MechanicalWorkshop workshop = new MechanicalWorkshop
        {
            Code = data.Code,
            CompanyName = data.CompanyName,
            RNC = data.RNC,
            Phone = data.Phone,
            Email = data.Email,
            Status = (int)GenericStatus.Activo
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

            workshop.AddressId = address.Id;
            await Data.Add( workshop);

           if (data.Contacts != null) {
                var concats = data.Contacts.Select(c => new Contact { Name = c.Name , Telephone =c.Phone , Email = c.Email, MechanicalWorkshopId = workshop.Id });
                await Data.AddRange(concats);
           }

           if (data.Specialties != null)
           {
              var specialties = data.Specialties.Select(c => new Specialty { Description = c.Description,  MechanicalWorkshopId = workshop.Id });
              await Data.AddRange(specialties);
           }
       }); 

        return workshop.Id;                                                 
    }

    public async Task<int> UpdateMechanicalWorkshop(MechanicalWorkshopData data, User user)
    {
        var workshop = await Data.GetMechanicalWorkshopById(data.Id);

        if (workshop == null) throw new NotFound("No se encontro suplidor");
        await Data.Atomic(async () => {

            workshop.Code = data.Code;
            workshop.CompanyName = data.CompanyName;
            workshop.RNC = data.RNC;
            workshop.Phone = data.Phone;
            workshop.Email = data.Email;
            workshop.Status = data.StatusId;
            await Data.Update( workshop,user.Id);

            workshop.Address.PlainAddress = $"{data.AddressLine1} {data.AddressLine2} {data.AddressLine3}";
            workshop.Address.AddressLine1 = data.AddressLine1;
            workshop.Address.AddressLine2 = data.AddressLine2;
            workshop.Address.AddressLine3 = data.AddressLine3;
            workshop.Address.CityId = data.CityId;

            await Data.Update(workshop.Address, user.Id);
            
            await ManagemmentContact(data.Contacts,workshop.Id,"MechanicalWorkshop" , workshop.Contacts.ToList());

            await ManagemmentSpecialty(data.Specialties,workshop.Id,"MechanicalWorkshop" , workshop.Specialties);
        });

        return workshop.Id;
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