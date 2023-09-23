using FleetTechCore.Model;

namespace FleetTechCore.Models.WorkShop
{
    public class MechanicalWorkshop:ServicePlace
    {
        public virtual ICollection<WorksopSpecialty> Specialties { get; set; }
    }
}
