namespace FleetTechCore.DTOs.Shared;

public record struct Item (int Id, string Description ){
   public static Item From<TEnum>(TEnum enum_value)
      where TEnum : struct, Enum {
      return new Item {
         Id = (int)(object)enum_value,
         Description = enum_value.ToString().PascalCaseWithInitialsToTitleCase()
      };
   }
}