using FleetTechCore.Models.Fuel;

namespace FleetTechCore.Models.Address
{
    public class Contact
    {
        public required int Id              { get; set; }
        public required string Name         { get; set; }
        public required string Telephone    { get; set; }
        public          string Email        { get; set; }

    }
}
