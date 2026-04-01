using Microsoft.EntityFrameworkCore;
using MedicamentosAPI.Models;

namespace MedicamentosAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Proveedor> proveedores { get; set; } // 🔥 NUEVO

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasPrecision(18, 2);

            // 🔥 CONFIGURAR RELACIÓN
            modelBuilder.Entity<Medicamento>()
                .HasOne(m => m.Proveedor)
                .WithMany(p => p.Medicamentos)
                .HasForeignKey(m => m.id_proveedor)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}