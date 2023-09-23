using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FleetTechCore.DTOs.Data
{
    public record struct ServicePlaseView
    (   int Id,
        string Code, 
        string CompanyName,
        string RNC,
        string Phone, 
        string Email,
        string AddressLine1, 
        string AddressLine2, 
        string AddressLine3,
        int    CityId,
        List<ContactData> Contacts
    );
}
