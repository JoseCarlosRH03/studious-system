namespace FleetTechCore.Models.Brigade;

public class Brigade : AuditableEntity {
   public Guid     UserId      { get; set; }
   public string   Number      { get; set; }
   public string   Type        { get; set; }
   public string   Specialty   { get; set; }
   public int      StatusId    { get; set; }
   public decimal? Latitude    { get; set; }
   public decimal? Longitude   { get; set; }


   public virtual ICollection<BrigadeMember> Members { get; set; }

   public virtual User.User          User   { get; set; }
   public virtual BrigadeStatus Status { get; set; }
}