using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicamentosAPI.Models
{
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Required]
        [StringLength(100)]
        public string Laboratorio { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [StringLength(50)]
        public string Unidad { get; set; } = string.Empty;

        [Required]
        public DateTime FechaVencimiento { get; set; }

        public bool RequiereReceta { get; set; }

        [Required]
        [StringLength(100)]
        public string UserUid { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}