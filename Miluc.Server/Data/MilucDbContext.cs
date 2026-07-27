using Microsoft.EntityFrameworkCore;
using Miluc.Server.Models;
using Miluc.Server.Models.AutorizacionModel;
using Miluc.Server.Models.ConfiguracionesServiceLayer;
using Miluc.Server.Models.LogsErrores;



namespace Miluc.Server.Data
{
    public class MilucDbContext : DbContext
    {
        public MilucDbContext(DbContextOptions<MilucDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Roles> Roles => Set<Roles>();
        public DbSet<Permisos> Permisos => Set<Permisos>();
        public DbSet<UsuarioRol> UsuarioRoles => Set<UsuarioRol>();
        public DbSet<RolPermiso> RolPermisos => Set<RolPermiso>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<TipoUsuario> TipoUsuario => Set<TipoUsuario>();
        public DbSet<UsuarioTipoUsuario> UsuarioTipoUsuario => Set<UsuarioTipoUsuario>();
        public DbSet<LogsErrores> LogsErrores => Set<LogsErrores>();
        public DbSet<UsuarioOTP> UsuarioOTP => Set<UsuarioOTP>();

        public DbSet<SisConfiguracionesGenerales> SisConfiguracionesGenerales => Set<SisConfiguracionesGenerales>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Llave compuesta para UsuarioRol
            modelBuilder.Entity<UsuarioRol>()
                .HasKey(ur => new { ur.IdUsuario, ur.IdRol });

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Usuario)
                .WithMany(u => u.UsuarioRoles)
                .HasForeignKey(ur => ur.IdUsuario);

            modelBuilder.Entity<UsuarioRol>()
                .HasOne(ur => ur.Rol)
                .WithMany(r => r.UsuarioRoles)
                .HasForeignKey(ur => ur.IdRol);

            // 2. Llave compuesta para RolPermiso
            modelBuilder.Entity<RolPermiso>()
                .HasKey(rp => new { rp.IdRol, rp.IdPermiso });

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Rol)
                .WithMany(r => r.RolPermisos)
                .HasForeignKey(rp => rp.IdRol);

            modelBuilder.Entity<RolPermiso>()
                .HasOne(rp => rp.Permiso)
                .WithMany(p => p.RolPermisos)
                .HasForeignKey(rp => rp.IdPermiso);

            // 3. Configuración RefreshToken
            modelBuilder.Entity<RefreshToken>()
                .HasKey(rt => rt.IdRefreshToken);


            // Definimos la Clave Compuesta de la tabla intermedia
            modelBuilder.Entity<UsuarioTipoUsuario>()
                .HasKey(ut => new { ut.IdUsuario, ut.IdTipoUsuario });

            // Configuración de las relaciones
            modelBuilder.Entity<UsuarioTipoUsuario>()
                .HasOne(ut => ut.Usuario)
                .WithMany(u => u.UsuarioTipoUsuario)
                .HasForeignKey(ut => ut.IdUsuario);

            modelBuilder.Entity<UsuarioTipoUsuario>()
                .HasOne(ut => ut.TipoUsuario)
                .WithMany(t => t.UsuarioTipoUsuario)
                .HasForeignKey(ut => ut.IdTipoUsuario);


            modelBuilder.Entity<UsuarioOTP>()
            .HasOne(o => o.Usuario)
            .WithMany(u => u.UsuarioOTP)
            .HasForeignKey(o => o.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SisConfiguracionesGenerales>()
          .ToTable("SisConfiguracionesGenerales").HasKey(c => c.ConfiguracionesGeneralesId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
