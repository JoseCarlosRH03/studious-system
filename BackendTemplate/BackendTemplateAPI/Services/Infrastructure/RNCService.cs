using System.Data;

namespace BackendTemplateAPI.Services.Infrastructure;

public class RNCService : DbContext, IRNCService
{
    public RNCService(DbContextOptions<RNCService> options) : base(options)
    { }
    public IDbConnection Connection => Database.GetDbConnection();
    public DbSet<CustomerDocument> Contributors { get; set; }

    public async Task<bool> IsEnabled()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Connection.ConnectionString))
            {
                return false;
            }
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }

    public async Task<string> RetrieveCompanyName(string national_id)
    {
        var contributor = await Contributors.FirstOrDefaultAsync(c => c.Document == national_id);
        if (contributor is null)
            throw new NotFound("No encontrado");
        var name = !string.IsNullOrWhiteSpace(contributor.Name)? contributor.Name.Trim() : contributor.Commercial_Name.Trim();
        return $"{name}";
    }
}
