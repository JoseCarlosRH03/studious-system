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
        public required   string   LicenseFileName         { get; set; }
        public            string   Phone                   { get; set; }
        public required   int      LicenseCategory_id      { get; set; }

        public virtual LicenseDrivers LicenseDrivers { get; set; }

    }
}
