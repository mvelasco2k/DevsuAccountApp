using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Repositories;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/movimientos")]
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
            catch (Microservice.Accounts.Exceptions.InsufficientFundsException)
            {
                return BadRequest(new { message = "Saldo no disponible" });
            }
        }

        [HttpGet("by-cuenta/{cuentaId}")]
        public async Task<IActionResult> GetByCuenta(int cuentaId)
        {
            var list = await _repo.GetMovimientosByCuentaAsync(cuentaId);
            return Ok(list);
        }
    }
}
