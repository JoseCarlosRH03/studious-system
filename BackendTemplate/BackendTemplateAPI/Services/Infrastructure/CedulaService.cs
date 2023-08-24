namespace BackendTemplateAPI.Services.Infrastructure;

public class CedulaService: DbContext, ICedulaService {
        public DbSet<Cedulado> Cedulados { get; set; }

        public CedulaService(DbContextOptions<CedulaService> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder model) {
            model.Entity<Cedulado>(c => {
                c.ToTable("CEDULADOS");
                c.HasKey(c => c.Cedula);
                c.Property(c => c.Cedula).HasColumnName("CEDULA");
                c.Property(c => c.Nombres).HasColumnName("NOMBRES");
                c.Property(c => c.Apellido1).HasColumnName("APELLIDO1");
                c.Property(c => c.Apellido2).HasColumnName("APELLIDO2");
                c.Property(c => c.DateOfBirth).HasColumnName("FECHA_NAC");
                c.Property(c => c.Sex).HasColumnName("SEXO");
            });
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
