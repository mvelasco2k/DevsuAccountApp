using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Repositories;
using Microservice.Accounts.Exceptions;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class MovimientosController : ControllerBase
    {
        private readonly IAccountRepository _repo;

        public MovimientosController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Movimiento movimiento)
        {
            try
            {
                var created = await _repo.CreateMovimientoAsync(movimiento);
                return CreatedAtAction(nameof(GetByCuenta), new { cuentaId = created.CuentaId }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InsufficientValueException)
            {
                return BadRequest(new { message = "Saldo no disponible" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("by-cuenta/{cuentaId}")]
        public async Task<IActionResult> GetByCuenta(int cuentaId, [FromQuery] DateTime? fechaInicio = null, [FromQuery] DateTime? fechaFin = null)
        {
            var result = await _repo.GetMovimientosByCuentaAsync(cuentaId, fechaInicio, fechaFin);
            return Ok(result);
        }
    }
}
