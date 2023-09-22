using FleetTechCore.DTOs.Views;
using FleetTechCore.Models.Shared;
using Microsoft.AspNetCore.Http;
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
        int         LicenseCategoryId,
        int         StatusId,
        string Name,
        string Type,
        string Dataurl,
        int Size
    );
}
