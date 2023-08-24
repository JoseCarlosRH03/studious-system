namespace BackendTemplateCore.Services.Infrastructure;

public interface ICedulaService
{
    Task<string> RetrieveCitizenName(string national_id);
}