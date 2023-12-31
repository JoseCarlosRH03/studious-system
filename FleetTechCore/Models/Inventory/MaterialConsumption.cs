﻿namespace FleetTechCore.Models.Inventory;

public class MaterialConsumption {
   public int Id { get; set; }
   public int MaterialId { get; set; }
   public int BrigadeId { get; set; }

   public decimal Amount { get; set; }
   
   public virtual Material Material { get; set; }
}