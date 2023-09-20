using FleetTechCore.DTOs.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data
{
    public record struct DriverData
    (
        int         Id,
        string      EmployeeCode,
        string      IdentityDocument,
        string      FirstName,
        string      LastName,
        DateTime    DateOfBirth,
        DateTime    ExpirationOfTheLicense,
        DateTime    DateOfHire,
        string      LicenseFileName,
        string      Phone,
        int         LicenseCategory_id,
        int         Status_id
    );
}
