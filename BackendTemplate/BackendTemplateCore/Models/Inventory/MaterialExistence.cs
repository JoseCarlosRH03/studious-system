namespace BackendTemplateCore.Models;

public class MaterialExistence {
   public int     Id         { get; set; }
   public int     MaterialId { get; set; }
   public Guid?   UserId     { get; set; }
   public decimal Amount     { get; set; }

   public virtual Material Material { get; set; }
   public virtual User?    User     { get; set; }
}