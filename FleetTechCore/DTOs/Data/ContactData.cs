using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Data
{
    public record struct ContactData
    (
        int     Id,
        string  Name,
        string  Phone,
        string  Email
    );
}
