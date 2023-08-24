namespace BackendTemplateCore.Services.Infrastructure;

public interface IReadoutService
{
    Task<bool> IsEnabled();
    Task<DifferentialReadoutsArrayResponse> ReadoutsAsync(string subscriptionNumber, DateTime startDate, DateTime endDate);
    Task<ReadoutsArrayResponse> ReadoutKWAvgAsync(string subscriptionNumber, DateTime startDate, DateTime endDate);
    Task<ConsumptionArrayResponse> ConsumptionAsync(string subscriptionNumber, DateTime startDate, DateTime endDate);
    Task<ConsumptionArrayResponse> ActualBySubscriptionAsync(string subscriptionNumber);
    Task<ConsumptionArrayResponse> ActualByCustomerAddressAsync(int customerAddress);
}

public class ConsumptionArrayResponse
{
    // public int Quantity { get; set; }
    public string ResponseString { get; set; }
    public bool Success { get; set; }
    public ICollection<ConsumptionData> Data { get; set; }
}

public class ConsumptionResponse
{
    public string ResponseString { get; set; }
    public bool Success { get; set; }
    public ConsumptionData Data { get; set; }
}

public class DifferentialReadoutsArrayResponse
{
    //public int Quantity { get; set; }
    public string ResponseString { get; set; }

    public bool Success { get; set; }
    public ICollection<DifferentialReadouts> Data { get; set; }
}

public class ReadoutsArrayResponse
{
    public int Quantity { get; set; }
    public string ResponseString { get; set; }

    public bool Success { get; set; }
    public ICollection<ReadoutResponse> Data { get; set; }
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

public class ConsumptionData {
    public int      Id                { get; set; }
    public int      CompanyId         { get; set; }
    public string   ClientId          { get; set; }
    public string   SuppyId           { get; set; }
    public DateTime Date              { get; set; }
    public int      PreviousReadoutId { get; set; }
    public int      CurrentReadoutId  { get; set; }
    public long     PeriodTicks       { get; set; }

    public TimeSpan Period => TimeSpan.FromTicks(PeriodTicks);

    public decimal? ConsumptionKWH   { get; set; }
    public decimal? ConsumptionKVARH { get; set; }
    public decimal? MaxV             { get; set; }
    public decimal? MaxKW            { get; set; }
    public decimal? MaxKWPeak        { get; set; }
    public decimal? MaxKWNonPeak     { get; set; }

    public ReadoutResponse PreviousReadout { get; set; }
    public ReadoutResponse CurrentReadout  { get; set; }
}