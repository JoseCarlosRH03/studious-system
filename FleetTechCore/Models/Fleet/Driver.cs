using FleetTechCore.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Fleet
{
    public class Driver: AuditableEntity
    {
        public required   string   EmployeeCode            { get; set; }
        public required   string   IdentityDocument        { get; set; }
        public required   string   FirstName               { get; set; }
        public required   string   LastName                { get; set; }
        public            DateTime DateOfBirth             { get; set; }
        public required   DateTime ExpirationOfTheLicense  { get; set; }
        public required   DateTime DateOfHire              { get; set; }
        public            int?      LicenseFileId           { get; set; }
        public            string   Phone                   { get; set; }
        public required   int      LicenseCategoryId       { get; set; }
        public required   int      Status                  { get; set; }  

        public virtual LicenseType LicenseCategory { get; set; }
        public virtual StorageFile LicenseFile { get; set; }

    }
}
