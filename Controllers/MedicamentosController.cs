using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicamentosAPI.Models;
using MedicamentosAPI.Data;

namespace MedicamentosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicamentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MedicamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicamento>>> GetMedicamentos([FromQuery] string userUid)
        {
            if (string.IsNullOrEmpty(userUid))
                return BadRequest(new { message = "UserUid es requerido" });

            var medicamentos = await _context.Medicamentos
                .Where(m => m.UserUid == userUid)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return Ok(medicamentos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicamento>> GetMedicamento(int id, [FromQuery] string userUid)
        {
            var medicamento = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserUid == userUid);

            if (medicamento == null)
                return NotFound(new { message = "Medicamento no encontrado" });

            return Ok(medicamento);
        }

        [HttpPost]
        public async Task<ActionResult<Medicamento>> PostMedicamento(Medicamento medicamento)
        {
            // 🔥 Si no viene id_proveedor, dejarlo como null
            if (medicamento.id_proveedor == 0)
            {
                medicamento.id_proveedor = null;
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            medicamento.CreatedAt = DateTime.UtcNow;
            medicamento.UpdatedAt = DateTime.UtcNow;

            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMedicamento),
                new { id = medicamento.Id, userUid = medicamento.UserUid },
                medicamento);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicamento(int id, Medicamento medicamento)
        {
            if (id != medicamento.Id)
                return BadRequest(new { message = "ID no coincide" });

            var existing = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserUid == medicamento.UserUid);

            if (existing == null)
                return NotFound(new { message = "Medicamento no encontrado" });

            existing.Nombre = medicamento.Nombre;
            existing.Descripcion = medicamento.Descripcion;
            existing.Laboratorio = medicamento.Laboratorio;
            existing.Precio = medicamento.Precio;
            existing.Stock = medicamento.Stock;
            existing.Unidad = medicamento.Unidad;
            existing.FechaVencimiento = medicamento.FechaVencimiento;
            existing.RequiereReceta = medicamento.RequiereReceta;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Medicamento actualizado" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicamento(int id, [FromQuery] string userUid)
        {
            var medicamento = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserUid == userUid);

            if (medicamento == null)
                return NotFound(new { message = "Medicamento no encontrado" });

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Medicamento eliminado" });
        }

        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int nuevoStock, [FromQuery] string userUid)
        {
            var medicamento = await _context.Medicamentos
                .FirstOrDefaultAsync(m => m.Id == id && m.UserUid == userUid);

            if (medicamento == null)
                return NotFound(new { message = "Medicamento no encontrado" });

            if (nuevoStock < 0)
                return BadRequest(new { message = "Stock no puede ser negativo" });

            medicamento.Stock = nuevoStock;
            medicamento.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Stock actualizado", nuevoStock });
        }
        // GET: api/medicamentos/with-proveedor
        [HttpGet("with-proveedor")]
        public async Task<ActionResult<IEnumerable<object>>> GetMedicamentosConProveedor([FromQuery] string userUid)
        {
            if (string.IsNullOrEmpty(userUid))
                return BadRequest(new { message = "UserUid es requerido" });

            var medicamentos = await _context.Medicamentos
                .Where(m => m.UserUid == userUid)
                .Include(m => m.Proveedor)
                .Select(m => new
                {
                    m.Id,
                    m.Nombre,
                    m.Descripcion,
                    m.Laboratorio,
                    m.Precio,
                    m.Stock,
                    m.Unidad,
                    m.FechaVencimiento,
                    m.RequiereReceta,
                    m.UserUid,
                    m.CreatedAt,
                    m.UpdatedAt,
                    m.id_proveedor,
                    Proveedor = m.Proveedor != null ? new
                    {
                        m.Proveedor.id_proveedor,
                        m.Proveedor.nombre,
                        m.Proveedor.telefono
                    } : null
                })
                .ToListAsync();

            return Ok(medicamentos);
        }

    }
}