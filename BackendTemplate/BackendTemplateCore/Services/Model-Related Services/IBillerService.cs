namespace BackendTemplateCore.Services.Model_Related_Services;

public interface IBillerService
{
    public Task ProcessBatch(int batchId, Guid userId, CancellationToken stoppingToken);
    public Task<(BillingBatchLineStatuses, string? message, int? invoiceId)> ProcessLine(IServiceProvider serviceProvider, DateTime date, int batchLineId, SemaphoreSlim semaphore, Guid userId, string batchNumber, int batchId, CancellationToken stoppingToken);
}

public class UnifiedConsumption {
    public DateTime FirstDate         { get; set; }
    public DateTime LastDate          { get; set; }

    public decimal ConsumptionKWH   { get; set; }
    public decimal ConsumptionKVARH { get; set; }
    public decimal MaxV             { get; set; }
    public decimal MaxKW            { get; set; }
    public decimal MaxKWPeak        { get; set; }
    public decimal MaxKWNonPeak     { get; set; }
}