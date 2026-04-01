using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicamentosAPI.Models;
using MedicamentosAPI.Data;

namespace MedicamentosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> GetProveedores([FromQuery] string userUid)
        {
            if (string.IsNullOrEmpty(userUid))
                return BadRequest(new { message = "UserUid es requerido" });

            var proveedores = await _context.proveedores
                .Where(p => p.userUid == userUid)
                .OrderByDescending(p => p.created_at)
                .ToListAsync();

            return Ok(proveedores);
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor>> GetProveedor(int id, [FromQuery] string userUid)
        {
            var proveedor = await _context.proveedores
                .FirstOrDefaultAsync(p => p.id_proveedor == id && p.userUid == userUid);

            if (proveedor == null)
                return NotFound(new { message = "Proveedor no encontrado" });

            return Ok(proveedor);
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> PostProveedor(Proveedor proveedor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            proveedor.created_at = DateTime.UtcNow;
            _context.proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProveedor), 
                new { id = proveedor.id_proveedor, userUid = proveedor.userUid }, 
                proveedor);
        }

        // PUT: api/proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProveedor(int id, Proveedor proveedor)
        {
            if (id != proveedor.id_proveedor)
                return BadRequest(new { message = "ID no coincide" });

            var existing = await _context.proveedores
                .FirstOrDefaultAsync(p => p.id_proveedor == id && p.userUid == proveedor.userUid);

            if (existing == null)
                return NotFound(new { message = "Proveedor no encontrado" });

            existing.nombre = proveedor.nombre;
            existing.telefono = proveedor.telefono;
            existing.email = proveedor.email;
            existing.direccion = proveedor.direccion;
            existing.contacto_nombre = proveedor.contacto_nombre;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Proveedor actualizado" });
        }

        // DELETE: api/proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id, [FromQuery] string userUid)
        {
            var proveedor = await _context.proveedores
                .FirstOrDefaultAsync(p => p.id_proveedor == id && p.userUid == userUid);

            if (proveedor == null)
                return NotFound(new { message = "Proveedor no encontrado" });

            _context.proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Proveedor eliminado" });
        }
    }
}