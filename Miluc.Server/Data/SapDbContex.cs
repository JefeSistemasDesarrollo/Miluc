using Microsoft.EntityFrameworkCore;
using Miluc.Server.Models.Sap;

namespace Miluc.Server.Data
{
    public class SapDbContex : DbContext
    {
        public SapDbContex(DbContextOptions<SapDbContex> options) : base(options)
        {
        }
        public DbSet<OcrdClienteSap> OCRD => Set<OcrdClienteSap>();
        //  public DbSet<OCRG> OCRG => Set<OCRG>();
        public DbSet<OITM>? OITM => Set<OITM>();//articulos 
        public DbSet<ITM1>? ITM1 => Set<ITM1>();//Precios de articulos
        public DbSet<OPLN>? OPLN => Set<OPLN>();//Precios de articulos
        public DbSet<OSLP>? OSLP => Set<OSLP>();//Vendedores
        public DbSet<OBPP>? OBPP => Set<OBPP>();//Grupos de RUTAS 
        public DbSet<OCTG>? OCTG => Set<OCTG>();//Condiciones de pago
        //public DbSet<ClienteSap> OCRD { get; set; }
        public DbSet<BusinessPartnerGroup>? OCRG { get; set; }
        public DbSet<OSPPrecioEspecialSap>? OSPP { get; set; }
        public DbSet<OSTC>? OSTC { get; set; } //impuestos 
        public DbSet<ORDR>? ORDR { get; set; }
        public DbSet<RDR1>? RDR1 { get; set; }
        public DbSet<OK1_PICK_TPLACAS>? OK1_PICK_TPLACAS { get; set; }
        public DbSet<HBT_REGIMTRIB>? HBT_REGIMTRIB { get; set; }
        public DbSet<HBT_TIPODOC>? HBT_TIPODOC { get; set; }
        public DbSet<HBT_MUNICIPIO>? HBT_MUNICIPIO { get; set; }

        //ciudad medios magneticos 
        public DbSet<BPCO_MU>? BPCO_MU { get; set; }

        //codigos postales 
        public DbSet<HBT_CODIGOSPOSTALES>? HBT_CODIGOSPOSTALES { get; set; }
        public DbSet<HBT_ACTIVIDADECO>? HBT_ACTIVIDADECO { get; set; }



        public DbSet<OK1_FE_RESPONFIS>? OK1_FE_RESPONFIS { get; set; }
        public DbSet<HBT_REGIMENFISCAL>? HBT_REGIMENFISCAL { get; set; }
        public DbSet<HBT_RESPFISCAL>? HBT_RESPFISCAL { get; set; }

        //HBT_RESPFISCAL
        public DbSet<OWHT>? OWHT { get; set; }
        public DbSet<CRD4>? CRD4 { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. CONFIGURACIÓN DE MAESTROS (OCRD, OSLP, etc.)
            // OCRD - Clientes
            modelBuilder.Entity<OcrdClienteSap>().ToTable("OCRD").HasKey(c => c.CardCode);
            // CRD1 - Direcciones de Clientes
            modelBuilder.Entity<CRD1>(entity =>
            {
                entity.ToTable("CRD1");
                entity.HasKey(e => new { e.CardCode, e.AdresType, e.Address });
                entity.Property(e => e.CardCode).HasColumnName("CardCode");
                entity.Property(e => e.Address).HasColumnName("Address");
                entity.Property(e => e.AdresType).HasColumnName("AdresType");
            });
            // 
            modelBuilder.Entity<OWHT>().ToTable("OWHT").HasKey(c => c.WTCode);
            //grpo de venta 
            modelBuilder.Entity<BusinessPartnerGroup>().ToTable("OCRG").HasKey(g => g.GroupCode);
            // OSLP - Vendedores (Se define la PK una sola vez)
            modelBuilder.Entity<OSLP>().ToTable("OSLP").HasKey(v => v.SlpCode);
            modelBuilder.Entity<OSLP>().Property(v => v.SlpCode).ValueGeneratedNever(); // SAP controla el ID, no es Identity de SQL
            // OPLN - Listas de Precios
            modelBuilder.Entity<OPLN>().ToTable("OPLN").HasKey(c => c.ListNum);
            // OBPP - Grupos de Rutas
            modelBuilder.Entity<OBPP>().ToTable("OBPP").HasKey(c => c.PrioCode);
            // OCTG - Condiciones de Pago
            modelBuilder.Entity<OCTG>().ToTable("OCTG").HasKey(c => c.GroupNum);
            // OITM - Artículos
            modelBuilder.Entity<OITM>().ToTable("OITM").HasKey(i => i.ItemCode);
            modelBuilder.Entity<OITM>().Property(i => i.SWeight1).HasColumnName("SWeight1").HasColumnType("decimal(18,2)");
            // ITM1 - Precios de Artículos
            modelBuilder.Entity<ITM1>().ToTable("ITM1").HasKey(p => new { p.ItemCode, p.PriceList });
            modelBuilder.Entity<ITM1>().Property(p => p.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
            // OSTC - Impuestos
            modelBuilder.Entity<OSTC>().ToTable("OSTC").HasKey(c => c.Code);
            // @BPCO_MU - Ciudades Medios Magnéticos
            modelBuilder.Entity<BPCO_MU>().ToTable("@BPCO_MU").HasKey(e => e.Code);
            // OSPP - Lista de Precios Especiales
            modelBuilder.Entity<OSPPrecioEspecialSap>().ToTable("OSPP").HasKey(p => new { p.ItemCode, p.CardCode });
            // @OK1_PICK_TPLACAS - Placas de Picking
            modelBuilder.Entity<OK1_PICK_TPLACAS>().ToTable("@OK1_PICK_TPLACAS").HasKey(p => p.code);
            //regimen tributario 
            modelBuilder.Entity<HBT_REGIMTRIB>().ToTable("@HBT_REGIMTRIB").HasKey(p => p.code);
            //DOCUMENTOS
            modelBuilder.Entity<HBT_TIPODOC>().ToTable("@HBT_TIPODOC").HasKey(d => d.Code);
            //municipios
            modelBuilder.Entity<HBT_MUNICIPIO>().ToTable("@HBT_MUNICIPIO").HasKey(m => m.Code);
            //responsabilidad fiscal
            modelBuilder.Entity<OK1_FE_RESPONFIS>().ToTable("@OK1_FE_RESPONFIS").HasKey(r => r.Code);
            //regimen fiscal
            modelBuilder.Entity<HBT_REGIMENFISCAL>().ToTable("@HBT_REGIMENFISCAL").HasKey(r => r.Code);
            //codigos postales
            modelBuilder.Entity<HBT_CODIGOSPOSTALES>().ToTable("@HBT_CODIGOSPOSTALES").HasKey(c => c.Code);

            modelBuilder.Entity<HBT_ACTIVIDADECO>().ToTable("@HBT_ACTIVIDADECO").HasKey(c => c.Code);
            modelBuilder.Entity<HBT_RESPFISCAL>().ToTable("@HBT_RESPFISCAL").HasKey(c => c.Code);

            modelBuilder.Entity<CRD4>().ToTable("CRD4").HasKey(c => new { c.CardCode, c.WTCode });

            // 2. CONFIGURACIÓN DE DOCUMENTOS (ORDR, RDR1)
            // ORDR - Órdenes de Venta (¡CORREGIDO: La PK real en SAP es DocEntry!)
            modelBuilder.Entity<ORDR>().ToTable("ORDR").HasKey(o => o.DocEntry);
            modelBuilder.Entity<ORDR>().Property(o => o.VatSum).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ORDR>().Property(o => o.DocRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ORDR>().Property(o => o.DocTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<ORDR>().Property(o => o.PaidToDate).HasColumnType("decimal(18,2)");
            // RDR1 - Detalle de Pedido
            modelBuilder.Entity<RDR1>().ToTable("RDR1").HasKey(d => new { d.DocEntry, d.LineNum });
            modelBuilder.Entity<RDR1>().Property(d => d.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<RDR1>().Property(d => d.LineTotal).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<RDR1>().Property(d => d.VatSum).HasColumnType("decimal(18,2)");
            // 3. MAPEO DE RELACIONES (NAVIGATIONS)
            // Relación: Artículos -> Precios
            modelBuilder.Entity<ITM1>().HasOne(p => p.OITM).WithMany(i => i.ITM1).HasForeignKey(p => p.ItemCode);
            modelBuilder.Entity<OPLN>().HasMany(c => c.ITM1).WithOne(p => p.OPLN).HasForeignKey(p => p.PriceList);
            // Relación: Precios Especiales -> Artículos
            modelBuilder.Entity<OSPPrecioEspecialSap>().HasOne(p => p.OITM).WithMany(i => i.OSPPrecioEspecialSap).HasForeignKey(p => p.ItemCode);
              // Relación: Lista de Precios -> Cliente
            modelBuilder.Entity<OPLN>().HasMany(c => c.OCRD).WithOne(g => g.OPLN).HasForeignKey(c => c.ListNum);
            // Relación: Vendedor (OSLP) -> Cliente (OCRD) (Corregido a WithMany)
            modelBuilder.Entity<OSLP>().HasMany(v => v.OCRD).WithOne(c => c.OSLP).HasForeignKey(c => c.SlpCode);
            // Relación: Grupo Clientes -> Cliente
            modelBuilder.Entity<OcrdClienteSap>().HasOne(c => c.OCRG).WithMany(g => g.OCRD).HasForeignKey(c => c.GroupCode);
            // Relación: Rutas -> Cliente
            modelBuilder.Entity<OBPP>().HasMany(c => c.OCRD).WithOne(c => c.OBPP).HasForeignKey(c => c.Priority);
            // Relación: Condiciones de pago -> Cliente
            modelBuilder.Entity<OCTG>().HasMany(c => c.OCRD).WithOne(g => g.OCTG).HasForeignKey(c => c.groupNum);
            // Relación: Cliente -> Direcciones
            modelBuilder.Entity<OcrdClienteSap>().HasMany(c => c.Direcciones).WithOne(d => d.OCRD).HasForeignKey(c => c.CardCode);
            // Relación: Ciudad Medios Magnéticos -> Direcciones
            modelBuilder.Entity<BPCO_MU>().HasMany(c => c.CRD1).WithOne(g => g.BPCO_MU).HasForeignKey(c => c.ZipCode);
            // Relación: Impuestos -> Artículos
            modelBuilder.Entity<OSTC>().HasMany(c => c.OITM).WithOne(o => o.OSTC).HasForeignKey(o => o.TaxCodeAR);
            // Relación: Orden de Venta -> Cliente
            modelBuilder.Entity<ORDR>().HasOne(o => o.OcrdClienteSap).WithMany(c => c.ORDR).HasForeignKey(o => o.CardCode);
            // Relación: Orden de Venta -> Detalle Orden (RDR1 se une por DocEntry perfectamente ahora)
            modelBuilder.Entity<RDR1>().HasOne(d => d.ORDR).WithMany(o => o.RDR1).HasForeignKey(d => d.DocEntry);
            // Relación: Placas Picking -> Orden de Venta
            modelBuilder.Entity<OK1_PICK_TPLACAS>().HasMany(p => p.ORDR).WithOne(o => o.piking).HasForeignKey(o => o.U_PLACAS).HasPrincipalKey(p => p.code).IsRequired(false);
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_TIPODOC).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_TipDoc);
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_REGIMTRIB).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_RegTrib);
            //relacion con responsabilidad fiscal
            //relacion con municipios 
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_MUNICIPIO).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_MunMed);
            //relacion con tipo de documento
            //modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.OK1_FE_RESPONFIS).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_ResFis);
            // Relación entre cliente y regimen fiscal
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_REGIMENFISCAL).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_RegFis);
            //codigos postales 
            //modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_CODIGOSPOSTALES).WithMany(o => o.OCRD).HasForeignKey(o => o.ZipCode).HasPrincipalKey(c => c.Code).IsRequired(false);
            // Relación exacta entre Orden de Venta (ORDR) y Vendedor (OSLP)
            modelBuilder.Entity<ORDR>().HasOne(o => o.OSLP).WithMany(v => v.ORDR).HasForeignKey(o => o.SlpCode).HasPrincipalKey(v => v.SlpCode);
            //RELACION DE ARTICULO CON DETELALE DEL LA ORDEN DE VENTA 
            // modelBuilder.Entity<RDR1>().HasOne(d=>d.OITM).WithMany(o => o.RDR1).HasForeignKey(o => o.ItemCode);
            modelBuilder.Entity<OITM>().HasMany(d => d.RDR1).WithOne(d => d.OITM).HasForeignKey(o => o.ItemCode);

            //ACTIVIDAD ECONOMICA 
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_ACTIVIDADECO).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_ActEco);

            //HBT_RESPFISCAL 
            modelBuilder.Entity<OcrdClienteSap>().HasOne(d => d.HBT_RESPFISCAL).WithMany(o => o.OCRD).HasForeignKey(o => o.U_HBT_ResFis);

            // Relación entre cliente y retenciones (CRD4)
            modelBuilder.Entity<OcrdClienteSap>().HasMany(c => c.CRD4).WithOne(d => d.OCRD).HasForeignKey(d => d.CardCode);
            modelBuilder.Entity<OWHT>().HasMany(c => c.CRD4).WithOne(d => d.OWHT).HasForeignKey(d => d.WTCode);

            //modelBuilder.Entity<CRD4>().HasOne(d => d.Cliente).WithMany(o => o.CRD4).HasForeignKey(o => o.CardCode);
            //modelBuilder.Entity<CRD4>().HasOne(d => d.Retencion).WithMany(o => o.CRD4).HasForeignKey(o => o.WTCode);
        }
    }
}
