namespace FleetTechCore.DTOs.Data;

public record struct BranchData(
    string Code,
    int CityId,
    int BranchTypeId,
    string Locality,
    string Address,
    string Phone);