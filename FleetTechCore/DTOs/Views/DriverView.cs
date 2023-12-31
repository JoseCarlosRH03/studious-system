﻿using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.Fleet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views;

    public record struct DriverView
    (
        int    Id,
        string EmployeeCode,
        string IdentityDocument,
        string FirstName,
        string LastName,
        DateTime DateOfBirth,
        DateTime ExpirationOfTheLicense,
        DateTime DateOfHire,
        string LicenseFileName,
        string Phone,
        Item Status,
        Item LicenseDrivers,
        int? FileId

    )
    {
        public static DriverView From(Driver data) => new()
        {
            Id = data.Id,
            EmployeeCode = data.EmployeeCode,
            IdentityDocument = data.IdentityDocument,
            FirstName = data.FirstName,
            LastName = data.LastName,
            DateOfBirth = data.DateOfBirth,
            DateOfHire = data.DateOfHire,
            ExpirationOfTheLicense = data.ExpirationOfTheLicense,
            Phone = data.Phone,
            Status =  new Item { Id = data.Status, Description = ((GenericStatus)data.Status).ToString()},
            LicenseDrivers = new Item {Id = data.LicenseCategory.Id, Description = data.LicenseCategory.Description},
            FileId = data?.LicenseFileId is null? 0 : data?.LicenseFileId
        };
    }
