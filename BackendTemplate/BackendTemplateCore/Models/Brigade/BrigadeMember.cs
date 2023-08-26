namespace BackendTemplateCore.Models.Brigade;

public class BrigadeMember {
   public int    Id         { get; set; }
   public string Name       { get; set; }
   public string IdDocument { get; set; }
   public string Specialty  { get; set; }

   public int? BrigadeId { get; set; }
   public virtual Brigade Brigade { get; set; }
}
