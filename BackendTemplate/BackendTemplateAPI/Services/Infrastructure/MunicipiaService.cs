using System.Text;
using System.Text.Json;

namespace BackendTemplateAPI.Services.Infrastructure;

public class MunicipiaService : IMunicipiaService
{
   public MunicipiaService(IDataService data)
   {
      Data = data;
   }
   private readonly IDataService Data;
   public async Task<MunicipiaLoginResponse> Login(string username, string password) {
      const string login_url = "/muniexternal/Auth/login";

      var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.MunicipiaService, e => e.Properties);
      if (extension == null)
         throw new NotFound("Extension no encontrada");

      var url = extension.Properties.FirstOrDefault(x => x.Name.Equals("Url"));
      if (url is null || string.IsNullOrWhiteSpace(url.Value))
         throw new NotFound("Url no esta configurado");
      if (string.IsNullOrWhiteSpace(username))
         throw new InvalidParameter("Username no puede estar vacio");
      if (string.IsNullOrWhiteSpace(password))
         throw new InvalidParameter("Password no puede estar vacia");

      var client = new HttpClient { BaseAddress = new Uri(url.Value) };

      var login = new MunicipiaLoginRequest {
         email = username,
         clave = password
      };

      var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
      var response = await client.PostAsync(login_url, content);
      
      if (!response.IsSuccessStatusCode) 
         throw new Exception(await response.Content.ReadAsStringAsync());
      
      var stringResponse = await response.Content.ReadAsStringAsync();
      var result = JsonSerializer.Deserialize<MunicipiaLoginResponse>(stringResponse);
      ExtensionProperty access_key;
      if (extension.Properties.Any(x => x.Name.Equals("AccessKey"))) {
         access_key = extension.Properties.First(x => x.Name.Equals("AccessKey"));
         extension.Properties.Remove(access_key);
      } else {
         access_key = new ExtensionProperty {
            Name = "AccessKey"
         };
      }
      access_key.Value = result.access_token;
      extension.Properties.Add(access_key);
      await Data.Update(extension);
      return result;

   }

   public async Task<(MunicipiaRegisteredInputResponse, string, DateTime)> RegisterInputs(string[]? args = null, DateTime? date = null) {
      try
      {
         var extension = await Data.GetAsync<Extension>(e => e.Id == (int) ExtensionIdentifiers.MunicipiaService,
            e => e.Properties);
         if (extension == null)
            throw new NotFound("Extension no encontrada");

         Console.WriteLine("Extension Found: " + extension.Name);


         var url      = extension.Properties.FirstOrDefault(p => p.Name.Equals("Url"));
         var apiKey   = extension.Properties.FirstOrDefault(p => p.Name.Equals("ApiKey"));
         var username = extension.Properties.FirstOrDefault(p => p.Name.Equals("Username"));
         var password = extension.Properties.FirstOrDefault(p => p.Name.Equals("Password"));
         var origin   = extension.Properties.FirstOrDefault(p => p.Name.Equals("Origin"));

         if (apiKey is null || string.IsNullOrWhiteSpace(apiKey.Value))
            throw new NotFound("ApiKey no esta configurado");
         if (username is null || string.IsNullOrWhiteSpace(username.Value))
            throw new NotFound("Username no esta configurado");
         if (password is null || string.IsNullOrWhiteSpace(password.Value))
            throw new NotFound("Password no esta configurado");
         if (origin is null || string.IsNullOrWhiteSpace(origin.Value))
            throw new NotFound("Origin no esta configurado");

         var accessKey = (await Login(username.Value, password.Value)).access_token;
         Console.WriteLine("Access Key Found: " + accessKey);

         try
         {
            date ??= DateTime.Parse(args.Length > 0
               ? args[0]
               : DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy-MM-dd"));
         }
         catch (Exception e)
         {
            date ??= DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
         }

         Console.WriteLine("Date Parsed: " + date.Value.ToString("yyyy-MM-dd"));

         #region Payment Data Collection

         var payments = await Data.GetPaymentListForDate(date.Value);
         var advancedPayments = await Data.GetAdvancedPaymentListForDate(date.Value);
         payments = payments.Where(p => p.Payment.StatusId == (int) PaymentStatuses.PagoAFactura).ToList();

         #endregion

         Console.WriteLine("Payments Found: " + payments.Count);
         Console.WriteLine("Advanced Payments Found: " + advancedPayments.Count);

         const string input_url = "/muniexternal/Ingresos/registro";

         var client = new HttpClient {BaseAddress = new Uri(url.Value)};
         client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessKey}");

         #region Compile payment Details

         var details = payments
            .SelectMany(p =>
               (p.Invoice.Lines.Any()
                  ? p.Invoice.Lines
                  : throw new NotFound("No se encontraron productos en la factura de la aplicacion de pago " + p.PaymentId))
               .Where(i => i.Product.ERPCode != null)
               .Select(i => new MunicipiaRegisteredInputDetails
               {
                  clasificador = i.Product.ERPCode.Trim(),
                  monto = (p.ApplyAmount / (p.Invoice.DocumentAmount + Math.Abs(p.Invoice.Lines
                     .Where(x => x.Product.ERPCode == null).Sum(x => x.TotalAmount))) * i.TotalAmount).ToString()
               }))
            .GroupBy(
               m => m.clasificador,
               m => m.monto,
               (c, a) => new MunicipiaRegisteredInputDetails
               {
                  clasificador = c,
                  monto = a.Select(Convert.ToDecimal).Sum().ToString("F2")
               })
            .ToList();
         details.AddRange(advancedPayments.SelectMany(p =>
               (p.Subscription.Lines.Any()
                  ? p.Subscription.Lines
                  : throw new Exception("No se encontraron productos en la suscripcion de la aplicacion de pago " +
                                        p.Id))
               .Where(i => i.Product.ERPCode != null)
               .Select(i => new MunicipiaRegisteredInputDetails
               {
                  clasificador = i.Product.ERPCode.Trim(),
                  monto = (p.DocumentAmount / (p.Subscription.Lines.Sum(x => x.Quantity * x.Product.UnitPrice) +
                                               Math.Abs(p.Subscription.Lines.Where(x => x.Product.ERPCode == null).Sum(
                                                  x => x.Quantity * x.Product.UnitPrice))) 
                           * i.Quantity * i.Product.UnitPrice).ToString()
               }))
            .GroupBy(
               m => m.clasificador,
               m => m.monto,
               (c, a) => new MunicipiaRegisteredInputDetails
               {
                  clasificador = c,
                  monto = a.Select(Convert.ToDecimal).Sum().ToString("F2")
               })
            .ToList());

         #endregion

         var register_input_request = new MunicipiaRegisteredInputRequest
         {
            apikey = apiKey.Value,
            origen = origin.Value,
            fecha = date.Value.ToString("yyyy-MM-dd"),
            efectivo = GetPaymentTypeValue(PaymentMethodTypes.Efectivo, payments, advancedPayments),
            cheques = GetPaymentTypeValue(PaymentMethodTypes.Cheque, payments, advancedPayments),
            tarjeta = GetPaymentTypeValue(PaymentMethodTypes.Tarjeta, payments, advancedPayments),
            transferencia = GetPaymentTypeValue(PaymentMethodTypes.Transferencia, payments, advancedPayments),
            otros = GetPaymentTypeValue(null, payments, advancedPayments),
            detalle = details.ToArray()
         };

         Console.WriteLine("-------");
         Console.WriteLine("Efectivo: " + register_input_request.efectivo);
         Console.WriteLine("Cheques: " + register_input_request.cheques);
         Console.WriteLine("Tarjeta: " + register_input_request.tarjeta);
         Console.WriteLine("Transferencia: " + register_input_request.transferencia);
         Console.WriteLine("Otros: " + register_input_request.otros);
         Console.WriteLine();
         Console.WriteLine("Suma de cuentas: " + (Convert.ToDecimal(register_input_request.cheques) +
                                                  Convert.ToDecimal(register_input_request.efectivo) +
                                                  Convert.ToDecimal(register_input_request.tarjeta) +
                                                  Convert.ToDecimal(register_input_request.transferencia) +
                                                  Convert.ToDecimal(register_input_request.otros)));
         Console.WriteLine("-------");
         Console.WriteLine("Suma de Detalle: " + register_input_request.detalle.Sum(a => Convert.ToDecimal(a.monto)));
         Console.WriteLine("-------");
         Console.WriteLine();

         foreach (var municipiaRegisters in register_input_request.detalle)
         {
            Console.WriteLine("-------");
            Console.WriteLine("Clasificador: " + municipiaRegisters.clasificador);
            Console.WriteLine("Monto: " + municipiaRegisters.monto);
            Console.WriteLine("-------");

         }

         if (register_input_request.detalle.Length == 0)
            throw new Exception("No hubo pagos realizados este dia");

         Console.WriteLine("Payments Adapted to Municipia Format: " + register_input_request.detalle.Length);
         var content = new StringContent(JsonSerializer.Serialize(register_input_request), Encoding.UTF8,
            "application/json");
         Console.WriteLine(content.ReadAsStringAsync().Result);
         var response = await client.PostAsync(input_url, content);
         Console.WriteLine("Content Received: " + await response.Content.ReadAsStringAsync());

         if (!response.IsSuccessStatusCode)
            throw new Exception(await response.Content.ReadAsStringAsync());
         
         var stringResponse = await response.Content.ReadAsStringAsync();
         var result = JsonSerializer.Deserialize<MunicipiaRegisteredInputResponse>(stringResponse);
         var post = content.ReadAsStringAsync().Result;
         LogService.LogServiceResponse(result, post, result.message, date.Value, "Municipia");
         return (result, post, date.Value);
         
      }
      catch (Exception e)
      {
         LogService.LogException(e, "Municipia", date.Value);
         throw;
      }
   }

   public async Task<string> PreviewInputs(string[] args = null, DateTime? date = null) {
      var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.MunicipiaService, e => e.Properties);
      if (extension == null)
         throw new NotFound("Extension no encontrada");
      
      Console.WriteLine("Extension Found: " + extension.Name);
      

      var url       = extension.Properties.FirstOrDefault(p => p.Name.Equals("Url"));
      var apiKey    = extension.Properties.FirstOrDefault(p => p.Name.Equals("ApiKey"));
      var username  = extension.Properties.FirstOrDefault(p => p.Name.Equals("Username"));
      var password  = extension.Properties.FirstOrDefault(p => p.Name.Equals("Password"));
      var origin    = extension.Properties.FirstOrDefault(p => p.Name.Equals("Origin"));
      
      if (apiKey is null || string.IsNullOrWhiteSpace(apiKey.Value))
         throw new Exception("ApiKey no encontrada");
      if (username is null || string.IsNullOrWhiteSpace(username.Value))
         throw new Exception("Username no encontrada");
      if (password is null || string.IsNullOrWhiteSpace(password.Value))
         throw new Exception("Password no encontrada");
      if (origin is null || string.IsNullOrWhiteSpace(origin.Value))
         throw new Exception("Origin no encontrada");
      
      
      var accessKey = (await Login(username.Value, password.Value)).access_token;
      Console.WriteLine("Access Key Found: " + accessKey);
      
      try
      {
         date ??= DateTime.Parse(args.Length > 0? args[0] : DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy-MM-dd"));
      }
      catch (Exception e)
      {
         date ??= DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
      }
      Console.WriteLine("Date Parsed: " + date.Value.ToString("yyyy-MM-dd"));

      #region Payment Data Collection
      var payments = await Data.GetPaymentListForDate(date.Value);
      var advancedPayments = await Data.GetAdvancedPaymentListForDate(date.Value);
      payments = payments.Where(p => p.Payment.StatusId == (int)PaymentStatuses.PagoAFactura).ToList();
      #endregion
      
      Console.WriteLine("Payments Found: " + payments.Count);  
      Console.WriteLine("Advanced Payments Found: " + advancedPayments.Count);

      var client = new HttpClient { BaseAddress = new Uri(url.Value) };
      client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {accessKey}");
      
      #region Compile payment Details
      var details = payments
         .SelectMany(p =>
            (p.Invoice.Lines.Any()
               ? p.Invoice.Lines
               : throw new NotFound("No se encontraron productos en la factura de la aplicacion de pago " + p.PaymentId))
            .Where(i => i.Product.ERPCode != null)
            .Select(i => new MunicipiaRegisteredInputDetails
            {
               clasificador = i.Product.ERPCode.Trim(),
               monto = (p.ApplyAmount / (p.Invoice.DocumentAmount + Math.Abs(p.Invoice.Lines.Where(x => x.Product.ERPCode == null).Sum(x => x.TotalAmount < 0? x.TotalAmount : p.ApplyAmount / p.Invoice.DocumentAmount * x.TotalAmount))) * i.TotalAmount).ToString()
            }))
         .GroupBy(
            m => m.clasificador,
            m => m.monto,
            (c, a) => new MunicipiaRegisteredInputDetails
            {
               clasificador = c,
               monto = a.Select(Convert.ToDecimal).Sum().ToString("F2")
            })
         .ToList();
      details.AddRange(advancedPayments.SelectMany(p =>
            (p.Subscription.Lines.Any()
               ? p.Subscription.Lines
               : throw new Exception("No se encontraron productos en la suscripcion de la aplicacion de pago " + p.Id))
            .Where(i => i.Product.ERPCode != null)
            .Select(i => new MunicipiaRegisteredInputDetails
            {
               clasificador = i.Product.ERPCode.Trim(),
               monto = (p.DocumentAmount / (p.Subscription.Lines.Sum(x => x.Quantity * x.Product.UnitPrice) + Math.Abs(p.Subscription.Lines.Where(x => x.Product.ERPCode == null).Sum(x => x.Quantity * x.Product.UnitPrice < 0? x.Quantity * x.Product.UnitPrice : p.DocumentAmount / p.Subscription.Lines.Sum(x => x.Quantity * x.Product.UnitPrice) * x.Quantity * x.Product.UnitPrice))) * i.Quantity * i.Product.UnitPrice).ToString()
            }))
         .GroupBy(
            m => m.clasificador,
            m => m.monto,
            (c, a) => new MunicipiaRegisteredInputDetails
            {
               clasificador = c,
               monto = a.Select(Convert.ToDecimal).Sum().ToString("F2")
            })
         .ToList());
      #endregion
      
      var register_input_request = new MunicipiaRegisteredInputRequest {
         apikey        = apiKey.Value,
         origen        = origin.Value,
         fecha         = date.Value.ToString("yyyy-MM-dd"),
         efectivo      = GetPaymentTypeValue(PaymentMethodTypes.Efectivo, payments, advancedPayments),
         cheques       = GetPaymentTypeValue(PaymentMethodTypes.Cheque, payments, advancedPayments),
         tarjeta       = GetPaymentTypeValue(PaymentMethodTypes.Tarjeta, payments, advancedPayments),
         transferencia = GetPaymentTypeValue(PaymentMethodTypes.Transferencia, payments, advancedPayments),
         otros         = GetPaymentTypeValue(null, payments, advancedPayments),
         detalle       = details.ToArray()
      };
      
      Console.WriteLine("-------");
      Console.WriteLine("Efectivo: " + register_input_request.efectivo);
      Console.WriteLine("Cheques: " + register_input_request.cheques);
      Console.WriteLine("Tarjeta: " + register_input_request.tarjeta);
      Console.WriteLine("Transferencia: " + register_input_request.transferencia);
      Console.WriteLine("Otros: " + register_input_request.otros);
      Console.WriteLine();
      Console.WriteLine("Suma de cuentas: " + (Convert.ToDecimal(register_input_request.cheques) + Convert.ToDecimal(register_input_request.efectivo) + Convert.ToDecimal(register_input_request.tarjeta) + Convert.ToDecimal(register_input_request.transferencia) + Convert.ToDecimal(register_input_request.otros)));
      Console.WriteLine("-------");
      Console.WriteLine("Suma de Detalle: " + register_input_request.detalle.Sum(a => Convert.ToDecimal(a.monto)));
      Console.WriteLine("-------");
      Console.WriteLine();
      
      foreach (var municipiaRegisters in register_input_request.detalle)
      {
         Console.WriteLine("-------");
         Console.WriteLine("Clasificador: " + municipiaRegisters.clasificador);
         Console.WriteLine("Monto: " + municipiaRegisters.monto);
         Console.WriteLine("-------");

      }
      if (register_input_request.detalle.Length == 0)
         throw new Exception("No hubo pagos realizados este dia");
      
      Console.WriteLine("Payments Adapted to Municipia Format: " + register_input_request.detalle.Length);
      return JsonSerializer.Serialize(register_input_request);
      
   }
}