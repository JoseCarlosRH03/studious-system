using System.Text.Json;

namespace BackendTemplateAPI.Services.Infrastructure;

public class ReadoutService : IReadoutService
{
    public ReadoutService(IDataService _dataService)
    {
        Data = _dataService;
    }
    
    readonly IDataService Data;
    
    public async Task<bool> IsEnabled() {
        var result = new List<Boolean> {await Data.GetByIdAsync<Extension>((int)ExtensionIdentifiers.ReadoutService) is not null};

        var properties = (await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties))?.Properties;
        result.Add( properties?.Any(x => x.Name == "Url" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        result.Add( properties?.Any(x => x.Name == "ApiKey" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        result.Add( properties?.Any(x => x.Name == "ApiKeyHeader" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        
        return result.All(x => x);
    }
    
    public async Task<DifferentialReadoutsArrayResponse> ReadoutsAsync(string subscriptionNumber, DateTime startDate, DateTime endDate)
    {
        var url = $"/readouts/client/{subscriptionNumber}/differential/from/{startDate:yyyy-MM-dd}/to/{endDate:yyyy-MM-dd}";

        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties);
        if (extension is null)
            throw new NotFound("La extensión no fue encontrada");
        
        var client = GetClient(extension);
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) 
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DifferentialReadoutsArrayResponse>(stringResponse);

        return result;

    }

    public async Task<ReadoutsArrayResponse> ReadoutKWAvgAsync(string subscriptionNumber, DateTime startDate, DateTime endDate)
    {
        var url = $"/readouts/client/{subscriptionNumber}/date_range/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}";

        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties);
        if (extension is null)
            throw new NotFound("La extensión no fue encontrada");
        
        var client = GetClient(extension);
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) 
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ReadoutsArrayResponse>(stringResponse);

        if (result.Data.Any())
            result.Quantity = Convert.ToInt32(result.Data.Average(x => x.ReadoutKW));

        return result;
    }
    
    public async Task<ConsumptionArrayResponse> ConsumptionAsync(string subscriptionNumber, DateTime startDate, DateTime endDate)
    {
        var url = $"/consumption/client/{subscriptionNumber}/date_range/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}";

        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties);
        if (extension is null)
            throw new NotFound("La extensión no fue encontrada");
        
        var client = GetClient(extension);
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode) 
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConsumptionArrayResponse>(stringResponse);

        return result;
    }

    public async Task<ConsumptionArrayResponse> ActualBySubscriptionAsync(string subscriptionNumber)
    {
        var url = $"/consumption/client/{subscriptionNumber}/actual";
        
        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties);
        if (extension is null)
            throw new NotFound("La extensión no fue encontrada");
        
        var client = GetClient(extension);
        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) 
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConsumptionArrayResponse>(stringResponse);
        
        return result;
    }
    
    public async Task<ConsumptionArrayResponse> ActualByCustomerAddressAsync(int customerAddress)
    {
        var url = $"/consumption/supply/{customerAddress}/actual";
        
        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.ReadoutService, e => e.Properties);
        if (extension is null)
            throw new NotFound("La extensión no fue encontrada");
        
        var client = GetClient(extension);
        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) 
            throw new Exception(await response.Content.ReadAsStringAsync());
        
        var stringResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ConsumptionArrayResponse>(stringResponse);
        
        return result;
    }

    private static HttpClient GetClient(Extension extension){

        if (!extension.Properties.Any(x => x.Name.Equals("Url")) || 
            !extension.Properties.Any(x => x.Name.Equals("ApiKeyHeader")) || 
            !extension.Properties.Any(x => x.Name.Equals("ApiKey")))
            throw new NotFound("Propiedades de extension no encontradas, no se puede realizar la operacion con el repositorio de lectura.");
        var properties = extension.Properties.ToDictionary(x => x.Name, x => x.Value);
        var url = properties["Url"];
        var apiKeyHeader = properties["ApiKeyHeader"];
        var apiKey = properties["ApiKey"];

        var client = new HttpClient { BaseAddress = new Uri(url) };
        client.DefaultRequestHeaders.TryAddWithoutValidation(apiKeyHeader, apiKey);

        return client;
    }
}