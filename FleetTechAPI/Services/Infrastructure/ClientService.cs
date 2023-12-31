﻿using System.Text.Json;
using FleetTechCore.Enums;
using FleetTechCore.Errors;
using FleetTechCore.Models.Extensions;
using FleetTechCore.Services;
using FleetTechCore.Services.Infrastructure;

namespace FleetTechAPI.Services.Infrastructure;

public class ClientService : IClientService
{
    public ClientService(IDataService _dataService)
    {
        Data = _dataService;
    }
    
    readonly IDataService Data;
    
    public async Task<bool> IsEnabled() {
        var result = new List<Boolean> {await Data.GetByIdAsync<Extension>((int)ExtensionIdentifiers.Central) is not null};

        var properties = (await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.Central, e => e.Properties))?.Properties;
        result.Add( properties?.Any(x => x.Name == "Url" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        result.Add( properties?.Any(x => x.Name == "ApiKey" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        result.Add( properties?.Any(x => x.Name == "ApiKeyHeader" && !string.IsNullOrWhiteSpace(x.Value)) ?? false );
        
        return result.All(x => x);
    }
    
    public async Task<DifferentialReadoutsArrayResponse> ReadoutsAsync(string subscriptionNumber, DateTime startDate, DateTime endDate)
    {
        var url = $"/readouts/client/{subscriptionNumber}/differential/from/{startDate:yyyy-MM-dd}/to/{endDate:yyyy-MM-dd}";

        var extension = await Data.GetAsync<Extension>(e => e.Id == (int)ExtensionIdentifiers.Central, e => e.Properties);
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