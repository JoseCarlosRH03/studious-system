namespace BackendTemplateCore.Models
{
    public class BrigadeGeolocation
    {
        public int      Id        { get; set; }
        public Guid     BrigadeId  { get; set; }
        public decimal  Latitude  { get; set; }
        public decimal  Longitude { get; set; }
    }
}
