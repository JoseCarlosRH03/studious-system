using System.Text;
using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Payments;
using BackendTemplateCore.Models.Products;
using BackendTemplateCore.Services.Model_Related_Services;

namespace BackendTemplateCore;

public static class Extensions {
   
   public static readonly Dictionary<int, string> NumberToMonths = new()
   {
      {1, "Enero"},
      {2, "Febrero"},
      {3, "Marzo"},
      {4, "Abril"},
      {5, "Mayo"},
      {6, "Junio"},
      {7, "Julio"},
      {8, "Agosto"},
      {9, "Septiembre"},
      {10, "Octubre"},
      {11, "Noviembre"},
      {12, "Diciembre"}
   };
   
   public static string Join(this IEnumerable<string> strings, string separator = "") =>
      string.Join(separator, strings);

   public static string Join(this IEnumerable<char> chars, string separator = "") =>
      string.Join(separator, chars);

   public static IEnumerable<TResult> Select<TKey, TValue, TResult>(this IDictionary<TKey, TValue> dict,
      Func<TKey, TValue, TResult> selector) =>
      dict.Select(pair => selector(pair.Key, pair.Value));
   
   
   public static int? GetInt(this string? value) => !int.TryParse(value, out var _) ? null : int.Parse(value);
   // get int array from string 1,2,3,4
   public static int[]? GetIntArray(this string? value) =>
      (value?.Contains(',') ?? false) ? 
         (value?.Split(',').Select(int.Parse).ToArray() ?? null) : 
         string.IsNullOrWhiteSpace(value)? null : new[] { int.Parse(value) };
   
   public static string[] PascalCaseToStrings(this string text) {
      var words = new List<string>();
      var word = new StringBuilder();
      foreach (var ch in text) {
         if (char.IsUpper(ch)) {
            if (word.Length > 0) {
               words.Add(word.ToString());
               word.Clear();
            }
            word.Append(ch);
         } else word.Append(ch);
      }
      if (word.Length > 0)
         words.Add(word.ToString());
      return words.ToArray();
   }

   static bool IsUpper(this string text) => text.All(ch => char.IsUpper(ch));

   public static string[] PascalCaseWithInitialsToStrings(this string text) {
      var words = new List<string>();
      var word = new StringBuilder();
      foreach (var ch in text) {
         if (char.IsUpper(ch)) {
            if (word.Length > 0 && !word.ToString().IsUpper()) {
               words.Add(word.ToString());
               word.Clear();
            }
            word.Append(ch);
         } else {
            if (word.ToString().IsUpper() && word.Length > 1) {
               words.Add(word.ToString().Substring(0, word.Length - 1));
               word.Remove(0, word.Length - 1);
            }
            word.Append(ch);
         }
      }
      if (word.Length > 0)
         words.Add(word.ToString());
      return words.ToArray();
   }

   public static string PascalCaseToSnakeCase(this string text) =>
      string.Join('_', text.PascalCaseToStrings().Select(s => s.ToLower()));

   static string Decapitalize(this string text, params string[] words) =>
      words.Aggregate(text, (current, word) => current.Replace($" {word}", $" {word.ToLower()}"));

   public static string PascalCaseToTitleCase(this string text, string language = "es") {
      var capitalized = string.Join(' ', text.PascalCaseToStrings()).Trim();
      return language switch {
         "en" => capitalized.Decapitalize("Of", "And", "A", "An", "The", "But", "For", "At", "By", "To"),
         "es" => capitalized.Decapitalize("Un", "Una", "La", "Los", "Las", "El", "Y", "Pero", "Para", "En", "Por", "Desde", "Hasta", "Del", "De", "A"),
         _ => capitalized
      }; 
   }

   public static string PascalCaseWithInitialsToTitleCase(this string text, string language = "es") {
      var capitalized = string.Join(' ', text.PascalCaseWithInitialsToStrings()).Trim();
      return language switch {
         "en" => capitalized.Decapitalize("Of", "And", "A", "An", "The", "But", "For", "At", "By", "To"),
         "es" => capitalized.Decapitalize("Un", "Una", "La", "Los", "Las", "El", "Y", "Pero", "Para", "En", "Por", "Desde", "Hasta", "Del", "De", "A"),
         _ => capitalized
      };
   }
   public static string? NullIfNothing(this string? text) => string.IsNullOrWhiteSpace(text) ? null : text;

   private static readonly string nl = Environment.NewLine;
   private const string ind = "\t";
   public static string ExceptionToString(Exception ex) =>
      string.Join(nl, new[] {
         $"Exception: {ex.GetType().Name}",
         $"Message: {ex.Message}",
         ex.InnerException is not null
            ? string.Join(nl, ExceptionToString(ex.InnerException).Split(nl).Select(l => $"{ind}{l}"))
            : null,
         $"Stack trace: {ex.StackTrace}"
      }.Where(l => l is not null));

   public static string FormatAsPhone(this string text) {
      var digits = string.Join(string.Empty, text.Where(char.IsDigit));
      return $"({digits.Substring(0, 3)}) {digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
   }
   
   public static Exception Unwrap(this Exception ex) => ex.InnerException is not null ? Unwrap(ex.InnerException) : ex;

   public static bool HasPermission(this User user, PermissionAreas area, PermissionTypes type)
   {
      return user.Roles.Any(r => r.Role.RolePermissions.Any(p => p.Permission.PermissionAreaId == (int)area && p.Permission.PermissionTypeId == (int)type)) ||
             // else if (user.Claims.Any(c => c.Permission.PermissionAreaId == (int)area && c.Permission.PermissionTypeId == (int)type))
             //    return;
             user.Roles.Any(r => r.Role.Name == "SuperAdmin");
   }
      public static string GetPaymentTypeValue(PaymentMethodTypes? type, List<PaymentApply> payments, List<Payment> advancedPayments)
   {
      payments = type == null ? payments.Where(p => p.Payment.PaymentMethod.PaymentMethodTypeId is < (int)PaymentMethodTypes.Efectivo or > (int)PaymentMethodTypes.Cheque).ToList() : payments.Where(p => p.Payment.PaymentMethod.PaymentMethodTypeId == (int)type).ToList();
      advancedPayments = type == null ? advancedPayments.Where(p => p.PaymentMethod.PaymentMethodTypeId is < (int)PaymentMethodTypes.Efectivo or > (int)PaymentMethodTypes.Cheque).ToList() : advancedPayments.Where(p => p.PaymentMethod.PaymentMethodTypeId == (int)type).ToList();
      
      return (payments.Select(p => p.ApplyAmount).Sum() + advancedPayments.Select(p => p.DocumentAmount).Sum()).ToString("F2");
   }
   
   public static string GetPropertyValueByName(ICollection<ExtensionProperty> properties, string name) {
      return properties.First(x => x.Name.Equals(name)).Value;
   }

   public static (decimal Value, decimal Consumption) GetRateValue(this UnifiedConsumption Consumption, string UnitOfMeasure, ProductRateScale[] scale, bool Fixed, bool Override)
   {
      decimal value;
      scale = scale.OrderBy(s => s.LowerQuantity).ToArray();

      decimal consumption = 0;
      if (UnitOfMeasure.ToLower().Contains("kwh"))
         consumption = Consumption.ConsumptionKWH;
      else if (UnitOfMeasure.ToLower().Contains("kwpeak"))
         consumption = Consumption.MaxKWPeak;
      else if (UnitOfMeasure.ToLower().Contains("kwnonpeak"))
         consumption = Consumption.MaxKWNonPeak;
      else if (UnitOfMeasure.ToLower().Contains("kw"))
         consumption = Consumption.MaxKW;
      else if (UnitOfMeasure.ToLower().Contains("kvarh"))
         consumption = Consumption.ConsumptionKVARH;
      else if (UnitOfMeasure.ToLower().Contains('v'))
         consumption = Consumption.MaxV;

      if (Fixed)
         value = scale.Last(s =>
            s.LowerQuantity <= consumption && (s.UpperQuantity is null || s.UpperQuantity >= consumption)).Price;
      else if (scale.Last().LowerQuantity < consumption && Override)
         value = scale.Last(s => s.LowerQuantity <= consumption).Price *
                 (scale.Last(s => s.LowerQuantity <= consumption).UpperQuantity is not null
                    ? scale.Last(s => s.LowerQuantity <= consumption).UpperQuantity!.Value
                    : consumption);
      else
         value = scale.Sum(s =>
            s.Price * (s.LowerQuantity - (s.UpperQuantity is not null && consumption > s.UpperQuantity.Value
               ? s.UpperQuantity.Value
               : consumption)));

      return (value, consumption);
   }
}