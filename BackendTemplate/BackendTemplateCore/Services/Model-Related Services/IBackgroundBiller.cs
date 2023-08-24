namespace BackendTemplateCore.Services.Model_Related_Services;

public interface IBackgroundBiller
{
    Task RunAllPendings(Guid userId, CancellationToken stoppingToken);
    Task RunProcessing(int batchId, Guid userId, CancellationToken stoppingToken);
    Task PauseProcessing(int batchId, Guid userId);
}