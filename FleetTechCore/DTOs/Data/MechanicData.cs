using FleetTechCore.DTOs.Shared;
using FleetTechCore.Enums;
using FleetTechCore.Models.WorkShop;

namespace FleetTechCore.DTOs.Data;

public record struct MechanicData
    (   
        int Id,
        string Code,
        string Phone,
        string Name,
        string Email,
        int StatusId,
        string JobTitle,
        List<SpecialityData> Specialties
     );
