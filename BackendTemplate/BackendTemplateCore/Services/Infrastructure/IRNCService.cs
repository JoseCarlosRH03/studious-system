using System.Data;

namespace BackendTemplateCore.Services.Infrastructure;

public interface IRNCService
{
    IDbConnection Connection { get; }
    Task<bool> IsEnabled();
    Task<string> RetrieveCompanyName(string national_id);
}


public class CustomerDocument
{
    public int Id { get; set; }
    public string Document { get; set; }
    public int Type { get; set; }
    public string Name { get; set; }
    public string Commercial_Name { get; set; }
    public string Activity { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}