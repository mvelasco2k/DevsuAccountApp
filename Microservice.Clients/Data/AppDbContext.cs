using Microsoft.EntityFrameworkCore;
using Microservice.Clients.Entities;

namespace Microservice.Clients.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Persona> Personas => Set<Persona>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(eb =>
            {
                eb.ToTable("Cliente");
                eb.HasKey(c => c.ClienteId);
                eb.Property(c => c.ClienteId).ValueGeneratedOnAdd();

                eb.Property(c => c.Contrasena)
                    .HasMaxLength(200)
                    .IsRequired(false);

                eb.Property(c => c.Estado)
                    .IsRequired();

                eb.HasOne(c => c.Persona)
                    .WithOne()
                    .HasForeignKey<Cliente>(c => c.PersonaId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Persona>(eb =>
            {
                eb.ToTable("Persona");

                eb.HasKey(p => p.PersonaId);
                eb.Property(p => p.PersonaId).ValueGeneratedOnAdd();

                eb.Property(p => p.Nombre)
                    .HasMaxLength(200)
                    .IsRequired();

                eb.Property(p => p.Genero)
                    .HasMaxLength(20)
                    .IsRequired(false);

                eb.Property(p => p.Edad);

                eb.Property(p => p.Identificacion)
                    .HasMaxLength(50)
                    .IsRequired();

                eb.Property(p => p.Direccion)
                    .HasMaxLength(200)
                    .IsRequired(false);

                eb.Property(p => p.Telefono)
                    .HasMaxLength(50)
                    .IsRequired(false);
            });
        }
    }
}
