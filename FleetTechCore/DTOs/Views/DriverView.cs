using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views
{
    public record struct DriverView
    (   
        string EmployeeCode,
        string IdentityDocument,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        DateTime ExpirationOfTheLicense,
        string LicenseFileName,
        string Phone,
        int LicenseCategory_id,
        LicenseTypeView LicenseDrivers
    )
    { 
        public static DriverView From(Driver data) => new ()
        {
            EmployeeCode = data.EmployeeCode,
            IdentityDocument = data.IdentityDocument,
            FirstName = data.FirstName,
            LastName = data.LastName,
            DateOfBirth = data.DateOfBirth,
            ExpirationOfTheLicense = data.ExpirationOfTheLicense,
            Phone = data.Phone,
            LicenseCategory_id = data.LicenseCategory_id,
            LicenseDrivers = LicenseTypeView.From( data.LicenseDrivers )
        };
    }

    public record struct LicenseTypeView 
    (
        int Id,
        string Description
    )
    {
        public static LicenseTypeView From(LicenseType data) => new()
        {
            Id = data.Id,
            Description = data.Description
        };
    }
}
