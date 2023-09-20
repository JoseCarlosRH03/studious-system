using FleetTechCore.Models.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data
{
    public record struct StationData
    (
        string Code, 
        string CompanyName,
        string RNC,
        string Phone, 
        string Email,
        string Address, 
        List<Contact> Contacts 
    );
}
