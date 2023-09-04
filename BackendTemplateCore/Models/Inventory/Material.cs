namespace FleetTechCore.Models.Inventory;

public class Material: AuditableEntity {
   public int   Id        { get; set; }

   public string Code        { get; set; }
   public string Name        { get; set; }
   public string Description { get; set; }
   public string Unit        { get; set; }

   public decimal Cost { get; set; }

   public virtual ICollection<MaterialExistence> Existences { get; set; } = new HashSet<MaterialExistence>();
}