namespace BackendTemplateCore.Services.Infrastructure;

public interface IClientService
{
    Task<bool> IsEnabled();
    Task<DifferentialReadoutsArrayResponse> ReadoutsAsync(string subscriptionNumber, DateTime startDate, DateTime endDate);
}


public class DifferentialReadoutsArrayResponse
{
    //public int Quantity { get; set; }
    public string ResponseString { get; set; }

    public bool Success { get; set; }
    public ICollection<DifferentialReadouts> Data { get; set; }
}

public class DifferentialReadouts
{
    public string MeterId { get; set; }
    public ReadoutResponse First { get; set; }
    public ReadoutResponse Last { get; set; }
}

public class ReadoutResponse
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public DateTime Date { get; set; }
    public string ClientId { get; set; }
    public string SupplyId { get; set; }
    public string MeterId { get; set; }
    public decimal? ReadoutKWH { get; set; }
    public decimal? ReadoutKVARH { get; set; }
    public decimal? ReadoutV { get; set; }
    public decimal? ReadoutKW { get; set; }
    public decimal? ReadoutKWPeak { get; set; }
    public decimal? ReadoutKWNonPeak { get; set; }
    public decimal? ReadoutIncomingActive { get; set; }
    public decimal? ReadoutOutgoingActive { get; set; }
    public decimal? ReadoutNetActive { get; set; }
    public decimal? Multiplier { get; set; }
    public string Source { get; set; }
}