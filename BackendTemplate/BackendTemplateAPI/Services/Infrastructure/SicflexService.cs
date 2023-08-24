using System.Text;
using System.Text.Json;

namespace BackendTemplateAPI.Services.Infrastructure;

public class SicflexService : ISicflexService
{
    public SicflexService(IDataService data)
    {
        Data = data;
    }
    private readonly IDataService Data;

    public async Task<(SicflexResponse, string, DateTime)> RegisterInputs(string[]? args = null, DateTime? date = null)
    {
        try
        {
            var extension = await Data.GetAsync<Extension>(e => e.Id == (int) ExtensionIdentifiers.SicflexService,
                e => e.Properties);

            if (extension is null)
                throw new NotFound("Extension no encontrada");

            var url = extension.Properties.FirstOrDefault(e => e.Name.Equals("Url"))?.Value;
            var apiKey = extension.Properties.FirstOrDefault(e => e.Name.Equals("ApiKey"))?.Value;
            var procedencia = extension.Properties.FirstOrDefault(e => e.Name.Equals("Procedencia"))?.Value;
            var customerSupplierId =
                extension.Properties.FirstOrDefault(e => e.Name.Equals("CustomerSupplierId"))?.Value;
            var debitAccount = extension.Properties.FirstOrDefault(e => e.Name.Equals("CuentaDebito"))?.Value;
            var creditAccount = extension.Properties.FirstOrDefault(e => e.Name.Equals("CuentaCredito"))?.Value;

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(procedencia) ||
                string.IsNullOrEmpty(customerSupplierId) || string.IsNullOrEmpty(debitAccount) ||
                string.IsNullOrEmpty(creditAccount))
                throw new NotFound("Faltan datos de configuración");

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

            #region Data Collection

            var payments = await Data.GetPaymentListForDate(date ?? DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)));
            var invoices = await Data.GetInvoiceListForDate(date ?? DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)));

            #endregion

            #region TodayCreditDebit

            var invoiceSummary = invoices
                .SelectMany(p => p.Lines
                    .Select(i => new
                    {
                        producto = i.Product.Name,
                        monto = i.TotalAmount
                    }))
                .GroupBy(m => m.producto, m => m.monto, (c, a) => new {producto = c, monto = a.Sum()});

            var paymentSummary = payments
                .SelectMany(p => p.Invoice.Lines
                    .Select(i => new
                    {
                        producto = i.Product.Name,
                        monto = p.ApplyAmount / p.Invoice.DocumentAmount * i.TotalAmount
                    }))
                .GroupBy(m => m.producto, m => m.monto, (c, a) => new {producto = c, monto = a.Sum()});

            #endregion

            #region SicflexAdapter (Cgadcs)

            var debits = paymentSummary.Select(p => new SicflexCgadcs
            {
                noLinea = 0,
                cgacc = new SicflexCgacc
                {
                    numeroCuenta = debitAccount
                },
                detalle = p.producto,
                debito = p.monto,
                credito = 0
            }).ToArray();

            var credits = invoiceSummary.Select(p => new SicflexCgadcs
            {
                noLinea = 0,
                cgacc = new SicflexCgacc
                {
                    numeroCuenta = creditAccount
                },
                detalle = p.producto,
                debito = 0,
                credito = p.monto
            }).ToArray();

            #endregion

            var date_string = date?.ToString("yyyy-MM-dd") ??
                              DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy-MM-dd");

            var register_input_request = new[]
            {
                new SicflexInsertRequest
                {
                    fecha = date_string,
                    document = new SicflexDocument {id = "ED01"},
                    numero = Convert.ToInt32(date_string.Replace("-", "")),
                    procedencia = procedencia,
                    customerSupplierId = Convert.ToInt32(customerSupplierId),
                    concepto = "Entrada de diario desde BillFast " + date_string,
                    currencyDefinition = new SicflexCurrencyDefinition
                    {
                        currencyCode = "DOP"
                    },
                    currencyRate = 1,
                    valor = 0,
                    cgadcs = new[] {debits, credits}.SelectMany(c => c).ToArray()
                }
            };

            if (register_input_request.Any(rir => rir.cgadcs.Length == 0))
                throw new Exception("No hubo pagos realizados este dia");


            var client = new HttpClient {BaseAddress = new Uri(url)};
            const string input_url = "/api/gl/documents?schema=";

            var content = new StringContent(JsonSerializer.Serialize(register_input_request), Encoding.UTF8,
                "application/json");
            var response = await client.PostAsync(url + input_url + apiKey, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception(await response.Content.ReadAsStringAsync());

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SicflexResponse>(stringResponse);
            var post = await response.Content.ReadAsStringAsync();
            LogService.LogServiceResponse(result, post, result?.message ?? "", date.Value, "Sicflex");
            return (result, post, date!.Value);
        }
        catch (Exception e)
        {
            LogService.LogException(e, "Sicflex", date);
            throw;
        }
    }

    public async Task<string> PreviewInputs(string[]? args = null, DateTime? date = null)
    {
        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.SicflexService, e => e.Properties);
        if (extension is null)
            throw new NotFound("Extension no encontrada");

        var url = extension.Properties.FirstOrDefault(e => e.Name.Equals("Url"))?.Value;
        var apiKey = extension.Properties.FirstOrDefault(e => e.Name.Equals("ApiKey"))?.Value;
        var procedencia = extension.Properties.FirstOrDefault(e => e.Name.Equals("Procedencia"))?.Value;
        var customerSupplierId = extension.Properties.FirstOrDefault(e => e.Name.Equals("CustomerSupplierId"))?.Value;
        var debitAccount = extension.Properties.FirstOrDefault(e => e.Name.Equals("CuentaDebito"))?.Value;
        var creditAccount = extension.Properties.FirstOrDefault(e => e.Name.Equals("CuentaCredito"))?.Value;

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(procedencia) || string.IsNullOrEmpty(customerSupplierId) || string.IsNullOrEmpty(debitAccount) || string.IsNullOrEmpty(creditAccount))
            throw new NotFound("Faltan datos de configuración");
        
        try
        {
            date ??= DateTime.Parse(args.Length > 0? args[0] : DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy-MM-dd"));
        }
        catch (Exception e)
        {
            date ??= DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
        }
        Console.WriteLine("Date Parsed: " + date.Value.ToString("yyyy-MM-dd"));
        
        #region Data Collection

        var payments = await Data.GetPaymentListForDate(date ?? DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)));
        var invoices = await Data.GetInvoiceListForDate(date ?? DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)));

        #endregion

        #region TodayCreditDebit

        var invoiceSummary = invoices
            .SelectMany(p => p.Lines
                .Select(i => new
                {
                    producto = i.Product.Name,
                    monto = i.TotalAmount
                }))
            .GroupBy(m => m.producto, m => m.monto, (c, a) => new {producto = c, monto = a.Sum()});

        var paymentSummary = payments
            .SelectMany(p => p.Invoice.Lines
                .Select(i => new
                {
                    producto = i.Product.Name,
                    monto = p.ApplyAmount / p.Invoice.DocumentAmount * i.TotalAmount
                }))
            .GroupBy(m => m.producto, m => m.monto, (c, a) => new {producto = c, monto = a.Sum()});

        #endregion

        #region SicflexAdapter (Cgadcs)

        var debits = paymentSummary.Select(p => new SicflexCgadcs
        {
            noLinea = 0,
            cgacc = new SicflexCgacc
            {
                numeroCuenta = debitAccount
            },
            detalle = p.producto,
            debito = p.monto,
            credito = 0
        }).ToArray();

        var credits = invoiceSummary.Select(p => new SicflexCgadcs
        {
            noLinea = 0,
            cgacc = new SicflexCgacc
            {
                numeroCuenta = creditAccount
            },
            detalle = p.producto,
            debito = 0,
            credito = p.monto
        }).ToArray();

        #endregion

        var date_string = date?.ToString("yyyy-MM-dd") ??
                          DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("yyyy-MM-dd");

        var register_input_request = new[]
        {
            new SicflexInsertRequest
            {
                fecha = date_string,
                document = new SicflexDocument {id = "ED01"},
                numero = Convert.ToInt32(date_string.Replace("-", "")),
                procedencia = procedencia,
                customerSupplierId = Convert.ToInt32(customerSupplierId),
                concepto = "Entrada de diario desde BillFast " + date_string,
                currencyDefinition = new SicflexCurrencyDefinition
                {
                    currencyCode = "DOP"
                },
                currencyRate = 1,
                valor = 0,
                cgadcs = new[] {debits, credits}.SelectMany(c => c).ToArray()
            }
        };

        if (register_input_request.Any(rir => rir.cgadcs.Length == 0))
            throw new Exception("No hubo pagos realizados este dia");


        return JsonSerializer.Serialize(register_input_request);
    }
}