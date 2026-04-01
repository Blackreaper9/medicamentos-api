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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicamento>()
                .Property(m => m.Precio)
                .HasPrecision(18, 2);
        }
    }
}