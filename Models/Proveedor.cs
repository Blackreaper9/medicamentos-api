using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicamentosAPI.Models
{
    public class Proveedor
    {
        [Key]
        public int id_proveedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [StringLength(20)]
        public string telefono { get; set; } = string.Empty;

        [StringLength(100)]
        public string email { get; set; } = string.Empty;

        [StringLength(200)]
        public string direccion { get; set; } = string.Empty;

        [StringLength(100)]
        public string contacto_nombre { get; set; } = string.Empty;

        public DateTime created_at { get; set; }

        [Required]
        [StringLength(100)]
        public string userUid { get; set; } = string.Empty;

        // Relación: Un proveedor tiene muchos medicamentos
        public ICollection<Medicamento>? Medicamentos { get; set; }
    }
}