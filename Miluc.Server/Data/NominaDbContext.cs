using Microsoft.EntityFrameworkCore;
using Miluc.Server.Models.Nomina;

namespace Miluc.Server.Data
{

    public class NominaDbContext : DbContext
    {
        public NominaDbContext(DbContextOptions<NominaDbContext> options) : base(options)
        {
        }

        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Vacuna> Vacunas { get; set; }
        public DbSet<EsquemaVacunacion> EsquemaVacunacion { get; set; }



        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        public DbSet<Parentesco> Parentesco { get; set; }
        public DbSet<InformacionFamiliar> InformacionFamiliar { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }
        public DbSet<TipoContrato> TipoContrato { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<ContratoLaboral> ContratoLaboral { get; set; }
        public DbSet<ContratoLaboralDetalle> ContratoLaboralDetalle { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<Eps> Eps { get; set; }
        public DbSet<Arl> Arl { get; set; }
        public DbSet<Afp> Afp { get; set; }
        public DbSet<CajaCompensacion> CajaCompensacion { get; set; }
        public DbSet<AfiliacionSeguridadSocial> AfiliacionSeguridadSocial { get; set; }
        public DbSet<Genero> Genero { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Deporte> Deporte { get; set; }
        public DbSet<CondicionMedica> CondicionMedica { get; set; }
        public DbSet<MedioTransporte> MedioTransportes { get; set; }
        public DbSet<TipoVivienda> TipoVivienda { get; set; }
        public DbSet<ClaseVivienda> ClaseVivienda { get; set; }
        public DbSet<NivelAcademico> NivelAcademico { get; set; }
        public DbSet<MatrizSociodemografica> MatrizSociodemografica { get; set; }
        public DbSet<Cargo> Cargos { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre EsquemaVacunacionId y Vacuna
            modelBuilder.Entity<Vacuna>().ToTable("vacuna");
            modelBuilder.Entity<EsquemaVacunacion>().ToTable("EsquemaVacunacion");

            //empleado 
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Empleado>().HasKey(e => e.EmpleadoId);



            modelBuilder.Entity<Vacuna>().HasKey(v => v.VacunaId);
            modelBuilder.Entity<EsquemaVacunacion>().HasKey(e => e.EsquemaVacunacionId);

            // Relacion muchos a muchos entre EsquemaVacunacion y Vacuna
            modelBuilder.Entity<Vacuna>().HasMany(e => e.EsquemaVacunacionModel).WithOne(v => v.Vacuna).HasForeignKey(e => e.VacunaId);

            // Empleado
            modelBuilder.Entity<Empleado>().ToTable("Empleado");
            modelBuilder.Entity<Empleado>().HasKey(e => e.EmpleadoId);
            modelBuilder.Entity<Empleado>().HasMany(e => e.ContratoLaboral).WithOne(c => c.Empleado).HasForeignKey(c => c.EmpleadoId);
            modelBuilder.Entity<Empleado>().HasOne(e => e.Municipio).WithMany(m => m.Empleados).HasForeignKey(e => e.CodigoMunicipio).HasPrincipalKey(m => m.Codigo);
            //modelBuilder.Entity<Empleado>().HasMany(e => e.AfiliacionSeguridadSocial).WithOne(a => a.Empleado).HasForeignKey(a => a.EmpleadoId);
            modelBuilder.Entity<Empleado>().HasMany(e => e.MatrizSociodemografica).WithOne(c => c.Empleado).HasForeignKey(c => c.EmpleadoId);
            modelBuilder.Entity<Empleado>().HasMany(e => e.EsquemaVacunacion).WithOne(c => c.Empleado).HasForeignKey(c => c.EmpleadoId);
            modelBuilder.Entity<Empleado>().HasMany(e => e.InformacionFamiliar).WithOne(c => c.Empleado).HasForeignKey(c => c.EmpleadoId);

            // EstadoCivil
            modelBuilder.Entity<EstadoCivil>().ToTable("EstadoCivil");
            modelBuilder.Entity<EstadoCivil>().HasKey(e => e.EstadoCivilId);
            modelBuilder.Entity<EstadoCivil>().HasMany(p => p.Empleado).WithOne(i => i.EstadoCivil).HasForeignKey(i => i.EstadoCivilId);

            // Parentesco 
            modelBuilder.Entity<Parentesco>().ToTable("Parentesco");
            modelBuilder.Entity<Parentesco>().HasKey(p => p.ParentescoId);

            // Informacion familiar
            modelBuilder.Entity<InformacionFamiliar>().ToTable("InformacionFamiliar");
            modelBuilder.Entity<InformacionFamiliar>().HasKey(i => i.InformacionFamiliarId);
            modelBuilder.Entity<Parentesco>().HasMany(p => p.InformacionFamiliarModel).WithOne(i => i.Parentesco).HasForeignKey(i => i.ParentescoId);

            // TipoDocumento
            modelBuilder.Entity<TipoDocumento>().ToTable("TipoDocumento");
            modelBuilder.Entity<TipoDocumento>().HasKey(t => t.TipoDocumentoId);
            modelBuilder.Entity<TipoDocumento>().HasMany(t => t.Empleado).WithOne(i => i.TipoDocumento).HasForeignKey(i => i.TipoDocumentoId);

            // Tipo contrato
            modelBuilder.Entity<TipoContrato>().ToTable("TipoContrato");
            modelBuilder.Entity<TipoContrato>().HasKey(t => t.TipoContratoId);
            modelBuilder.Entity<TipoContrato>().HasMany(t => t.ContratoLaboralModel).WithOne(c => c.TipoContrato).HasForeignKey(c => c.TipoContratoId);

            // Empresa
            modelBuilder.Entity<Empresa>().ToTable("Empresa");
            modelBuilder.Entity<Empresa>().HasKey(e => e.EmpresaId);
            modelBuilder.Entity<Empresa>().HasMany(e => e.ContratoLaboralModel).WithOne(c => c.Empresa).HasForeignKey(c => c.EmpresaId);

            // Contrato laboral
            modelBuilder.Entity<ContratoLaboral>().ToTable("ContratoLaboral");
            modelBuilder.Entity<ContratoLaboral>().HasKey(c => c.ContratoLaboralId);
            modelBuilder.Entity<ContratoLaboral>().HasMany(c => c.ContratoLaboralDetalle).WithOne(d => d.ContratoLaboral).HasForeignKey(d => d.ContratoLaboralId);

            // Contrato laboral Detalle
            modelBuilder.Entity<ContratoLaboralDetalle>().ToTable("ContratoLaboralDetalle");
            modelBuilder.Entity<ContratoLaboralDetalle>().HasKey(c => c.ContratoLaboralDetalleId);
            modelBuilder.Entity<ContratoLaboralDetalle>().Property(x => x.Salario).HasPrecision(18, 2);

            modelBuilder.Entity<Cargo>().ToTable("Cargos");
            modelBuilder.Entity<Cargo>().HasKey(c => c.CargoId); // Coincide con la columna de tu imagen

            modelBuilder.Entity<ContratoLaboralDetalle>()
                .HasOne(d => d.Cargo)
                .WithMany(c => c.ContratoLaboralDetalle)
                .HasForeignKey(d => d.CargoId); // Asegúrate de que la FK en detalle apunte bien


            // Departamento
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<Departamento>().HasKey(d => d.Codigo);
            modelBuilder.Entity<Departamento>().HasMany(d => d.Municipio).WithOne(c => c.Departamento).HasForeignKey(d => d.CodigoDpto);

            // Municipio
            modelBuilder.Entity<Municipio>().ToTable("Municipio");
            modelBuilder.Entity<Municipio>().HasKey(d => d.Codigo);

            // Eps
            modelBuilder.Entity<Eps>().ToTable("Eps");
            modelBuilder.Entity<Eps>().HasKey(e => e.EpsId);
            modelBuilder.Entity<Eps>().HasMany(e => e.AfiliacionSeguridadSocial).WithOne(c => c.Eps).HasForeignKey(c => c.EpsId);

            // Arl
            modelBuilder.Entity<Arl>().ToTable("Arl");
            modelBuilder.Entity<Arl>().HasKey(a => a.ArlId);
            modelBuilder.Entity<Arl>().HasMany(a => a.AfiliacionSeguridadSocial).WithOne(c => c.Arl).HasForeignKey(c => c.ArlId);

            // Afp
            modelBuilder.Entity<Afp>().ToTable("Afp");
            modelBuilder.Entity<Afp>().HasKey(a => a.AfpId);
            modelBuilder.Entity<Afp>().HasMany(a => a.AfiliacionSeguridadSocial).WithOne(c => c.Afp).HasForeignKey(c => c.AfpId);

            // Caja Compensacion
            modelBuilder.Entity<CajaCompensacion>().ToTable("CajaCompensacion");
            modelBuilder.Entity<CajaCompensacion>().HasKey(c => c.CajaCompensacionId);
            modelBuilder.Entity<CajaCompensacion>().HasMany(a => a.AfiliacionSeguridadSocial).WithOne(c => c.CajaCompensacion).HasForeignKey(c => c.CajaCompensacionId);

            // AfiliacionSeguridadSocial
            modelBuilder.Entity<AfiliacionSeguridadSocial>().ToTable("AfiliacionSeguridadSocial");
            modelBuilder.Entity<AfiliacionSeguridadSocial>().HasKey(a => a.AfiliacionId);

            // Genero
            modelBuilder.Entity<Genero>().ToTable("Genero");
            modelBuilder.Entity<Genero>().HasKey(g => g.GeneroId);
            modelBuilder.Entity<Genero>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.Genero).HasForeignKey(d => d.GeneroId);

            // Pais
            modelBuilder.Entity<Pais>().ToTable("Pais");
            modelBuilder.Entity<Pais>().HasKey(g => g.PaisId);
            modelBuilder.Entity<Pais>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.Pais).HasForeignKey(d => d.PaisNacimientoId);

            // Deportes
            modelBuilder.Entity<Deporte>().ToTable("Deporte");
            modelBuilder.Entity<Deporte>().HasKey(d => d.DeporteId);
            modelBuilder.Entity<Deporte>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.Deporte).HasForeignKey(d => d.DeporteId);

            // CondicionMedica
            modelBuilder.Entity<CondicionMedica>().ToTable("CondicionMedica");
            modelBuilder.Entity<CondicionMedica>().HasKey(c => c.CondicionMedicaId);
            modelBuilder.Entity<CondicionMedica>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.CondicionMedica).HasForeignKey(d => d.CondicionMedicaId);

            // MedioTransporte
            modelBuilder.Entity<MedioTransporte>().ToTable("MedioTransporte");
            modelBuilder.Entity<MedioTransporte>().HasKey(m => m.MedioTransporteId);
            modelBuilder.Entity<MedioTransporte>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.MedioTransporte).HasForeignKey(d => d.MedioTransporteId);

            // TipoVivienda
            modelBuilder.Entity<TipoVivienda>().ToTable("TipoVivienda");
            modelBuilder.Entity<TipoVivienda>().HasKey(m => m.TipoViviendaId);
            modelBuilder.Entity<TipoVivienda>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.TipoVivienda).HasForeignKey(d => d.TipoViviendaId);

            // ClaseVivienda
            modelBuilder.Entity<ClaseVivienda>().ToTable("ClaseVivienda");
            modelBuilder.Entity<ClaseVivienda>().HasKey(m => m.ClaseViviendaId);
            modelBuilder.Entity<ClaseVivienda>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.ClaseVivienda).HasForeignKey(d => d.ClaseDeViviendaId);

            // NivelAcademico
            modelBuilder.Entity<NivelAcademico>().ToTable("NivelAcademico");
            modelBuilder.Entity<NivelAcademico>().HasKey(n => n.NivelAcademicoId);
            modelBuilder.Entity<NivelAcademico>().HasMany(g => g.MatrizSociodemografica).WithOne(c => c.NivelAcademico).HasForeignKey(d => d.NivelAcademicoId);

            // MatrizSociodemografica
            modelBuilder.Entity<MatrizSociodemografica>().ToTable("MatrizSociodemografica");
            modelBuilder.Entity<MatrizSociodemografica>().HasKey(m => m.MatrizSociodemograficaID);

            // SOLUCIÓN AL WARNING: Definición de precisión para UltimoSalario
            modelBuilder.Entity<MatrizSociodemografica>()
                .Property(m => m.UltimoSalario)
                .HasPrecision(18, 2);
        }


    }
}
