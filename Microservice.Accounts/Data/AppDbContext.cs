using Microsoft.EntityFrameworkCore;
using Microservice.Accounts.Entities;

namespace Microservice.Accounts.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cuenta> Cuentas => Set<Cuenta>();
        public DbSet<Movimiento> Movimientos => Set<Movimiento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cuenta>(eb =>
            {
                eb.ToTable("Cuenta");
                eb.HasKey(c => c.CuentaId);
                eb.Property(c => c.CuentaId).ValueGeneratedOnAdd();

                eb.Property(c => c.NumeroCuenta)
                    .HasMaxLength(50)
                    .IsRequired();

                eb.Property(c => c.TipoCuenta)
                    .HasMaxLength(50)
                    .IsRequired(false);

                eb.Property(c => c.SaldoInicial)
                    .HasColumnType("decimal(18,2)")
                    .HasDefaultValue(0m);
            });

            modelBuilder.Entity<Movimiento>(eb =>
            {
                eb.ToTable("Movimiento");
                eb.HasKey(m => m.MovimientoId);
                eb.Property(m => m.MovimientoId).ValueGeneratedOnAdd();

                eb.Property(m => m.Valor).HasColumnType("decimal(18,2)");
                eb.Property(m => m.Saldo).HasColumnType("decimal(18,2)");
                eb.Property(m => m.TipoMovimiento).HasMaxLength(50).IsRequired(false);
                eb.Property(m => m.Fecha).IsRequired();

                eb.HasOne(m => m.Cuenta)
                    .WithMany(c => c.Movimientos)
                    .HasForeignKey(m => m.CuentaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
