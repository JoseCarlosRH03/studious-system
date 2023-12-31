﻿using FleetTechCore.Models.Address;

namespace FleetTechCore.Models.Company;

public class Branch: AuditableEntity
{
    public string Code { get; set; }
    public int BranchTypeId { get; set; }
    public int CityId { get; set; }
    public string Locality { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int Status { get; set; }
    
    public virtual City City { get; set; }
    public virtual BranchType BranchType { get; set; }
}