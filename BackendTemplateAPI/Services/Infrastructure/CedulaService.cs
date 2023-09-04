using System.Data;
using FleetTechCore.Errors;
using FleetTechCore.Services.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FleetTechAPI.Services.Infrastructure;

public class CedulaService: DbContext, ICedulaService {
        public CedulaService(DbContextOptions<CedulaService> options) : base(options) { }
        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<Cedulado> Cedulados { get; set; }
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

        public async Task<string> RetrieveCitizenName(string national_id) {
            var cedulado = await Cedulados.FirstOrDefaultAsync(c => c.Cedula == national_id);
            if (cedulado is null)
                throw new NotFound("No se encontro el nombre de esa cedula, favor introducirlo manualmente o verificar la cedula");
            return $"{cedulado.Nombres} {cedulado.Apellido1} {cedulado.Apellido2}";
        }
    }
public class Cedulado {
    public string   Cedula      { get; set; }
    public string   Nombres     { get; set; }
    public string   Apellido1   { get; set; }
    public string   Apellido2   { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string   Sex         { get; set; }
}