using Microsoft.EntityFrameworkCore;
using MiWebService.Models;

namespace MiWebService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Persona> usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.APaterno)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.AMaterno)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Telefono)
                    .IsRequired();

                entity.Property(e => e.Correo)
                    .HasMaxLength(150)
                    .IsRequired(false);

                entity.Property(e => e.NameTag)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("dt_fecha_registro")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("dt_fecha_modificacion")
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Fecha)
                    .HasColumnName("dt_fecha")
                    .HasColumnType("date")
                    .HasDefaultValueSql("CURRENT_DATE");

                entity.Property(e => e.EnUso)
                    .HasColumnName("bool_enuso")
                    .HasColumnType("boolean")
                    .HasDefaultValue(true);

                entity.HasIndex(e => e.Telefono).IsUnique();

                entity.HasIndex(e => e.Correo)
                    .IsUnique()
                    .HasFilter("\"Correo\" IS NOT NULL");

                entity.HasIndex(e => e.NameTag)
                    .IsUnique()
                    .HasFilter("\"NameTag\" IS NOT NULL");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<Persona>().Where(e => e.State == EntityState.Modified))
            {
                entity.Entity.FechaModificacion = DateTime.Now;
            }
        
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            foreach (var entity in ChangeTracker.Entries<Persona>().Where(e => e.State == EntityState.Modified))
            {
                entity.Entity.FechaModificacion = DateTime.Now;
            }

            return base.SaveChanges();
        }
    }
}
