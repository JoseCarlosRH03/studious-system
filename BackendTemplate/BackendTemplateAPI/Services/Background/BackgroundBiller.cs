using System.Collections.Concurrent;

namespace BackendTemplateAPI.Services.Background;

public class BackgroundBiller : BackgroundService, IBackgroundBiller
{
    public BackgroundBiller(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }

    private readonly IServiceProvider Services;
    private Guid userId { get; set; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);

    private static ConcurrentDictionary<int, CancellationTokenSource> BatchTokens = new();

    private static void AddToken(int batchId, CancellationTokenSource token)
    {
        BatchTokens.TryAdd(batchId, token);
    }

    private static void GetToken(int batchId, out CancellationTokenSource? token)
    {
        token = BatchTokens.TryGetValue(batchId, out var tokenSource) ? tokenSource : default;
    }

    private static void RemoveToken(int batchId)
    {
        if (BatchTokens.TryRemove(batchId, out var tokenSource))
        {
            tokenSource.Dispose();
        }
    }



    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        try
        {
            using var scope = Services.CreateScope();
            var Data = scope.ServiceProvider.GetRequiredService<IDataService>();

            var getPendingBatches = await Data.GetAll<BillingBatch>(b => b.Status == (int) BillingBatchStatuses.Pending);
            if (getPendingBatches is not null && getPendingBatches.Any())
            {
                foreach (var batch in getPendingBatches)
                {

                    AddToken(batch.Id, tokenSource);
                    await Task.Run(async () =>
                    {
                        var biller = scope.ServiceProvider.GetRequiredService<IBillerService>();
                        batch.InvoicedOn = DateTime.Now;
                        batch.Status = (int) BillingBatchStatuses.Processing;
                        await Data.Update(batch, userId);

                        await biller.ProcessBatch(batch.Id, userId, tokenSource.Token);
                    }, tokenSource.Token);
                    batch.Status = (int) BillingBatchStatuses.Completed;
                    batch.EndDate = DateTime.Now;
                    await Data.Update(batch, userId);
                    RemoveToken(batch.Id);
                }
            }
        }
        finally
        {
            tokenSource.Dispose();
        }
    }
    
    public Task RunAllPendings(Guid userId, CancellationToken stoppingToken) => Task.Run(async () =>
    {
        this.userId = userId;
        await ExecuteAsync(stoppingToken);
    }, stoppingToken);
    
    public Task RunProcessing(int batchId, Guid userId, CancellationToken stoppingToken) => Task.Run(async () =>
    {
        this.userId = userId;
        using var scope = Services.CreateScope();
        
        var Data = scope.ServiceProvider.GetRequiredService<IDataService>();
        var bbatch = await Data.GetAsync<BillingBatch>(b => b.Id == batchId);
        if (bbatch is null)
            throw new NotFound("Lote de facturación no encontrado");
        try
        {
            switch (bbatch.Status)
            {
                case (int)BillingBatchStatuses.Cancelled:
                    throw new InvalidParameter("Este lote de facturación ha sido cancelado");
                case (int)BillingBatchStatuses.Completed:
                    throw new InvalidParameter("Este lote de facturación ya ha sido completado");
                case (int)BillingBatchStatuses.Errored:
                    throw new InvalidParameter("Este lote de facturación ha sido marcado como erróneo");
                default:
                {
                    var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                    AddToken(batchId, tokenSource);
                
                    var biller = scope.ServiceProvider.GetRequiredService<IBillerService>();
                    bbatch.InvoicedOn = DateTime.Now;
                    bbatch.Status = (int) BillingBatchStatuses.Processing;
                    await Data.Update(bbatch, userId);
                    await biller.ProcessBatch(bbatch.Id, userId, tokenSource.Token);
                    break;
                }
            }

        }
        finally
        {
            RemoveToken(batchId);
        }
    }, stoppingToken);

    public async Task PauseProcessing(int batchId, Guid userId) => Task.Run(async () =>
    {
        this.userId = userId;
        using var scope = Services.CreateScope();

        var Data = scope.ServiceProvider.GetRequiredService<IDataService>();
        var bbatch = await Data.GetAsync<BillingBatch>(b => b.Id == batchId);
        if (bbatch is null)
            throw new NotFound("Lote de facturación no encontrado");

        switch (bbatch.Status)
        {
            case (int)BillingBatchStatuses.Cancelled:
                throw new InvalidParameter("Este lote de facturación ha sido cancelado");
            case (int)BillingBatchStatuses.Completed:
                throw new InvalidParameter("Este lote de facturación ya ha sido completado");
            case (int)BillingBatchStatuses.Errored:
                throw new InvalidParameter("Este lote de facturación ha sido marcado como erróneo");
            case (int)BillingBatchStatuses.Pending:
                throw new InvalidParameter("Este lote de facturación no ha sido procesado");
            default:
            {
                GetToken(batchId, out var token);
                if (token != default)
                {
                    token.Cancel();
                    RemoveToken(batchId);
                }
                bbatch.Status = (int)BillingBatchStatuses.Pending;
                await Data.Update(bbatch, userId);
                break;
            }
        }
    });
            
}