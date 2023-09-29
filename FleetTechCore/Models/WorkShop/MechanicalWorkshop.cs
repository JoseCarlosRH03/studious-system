using FleetTechCore.Model;

namespace FleetTechCore.Models.WorkShop
{
    public class MechanicalWorkshop:ServicePlace
    {
        public virtual ICollection<Specialty> Specialties { get; set; }
    }
}
