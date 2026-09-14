using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Repositories;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CuentasController : ControllerBase
    {
        private readonly IAccountRepository _repo;

        public CuentasController(IAccountRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cuenta cuenta)
        {
            var created = await _repo.CreateCuentaAsync(cuenta);
            return CreatedAtAction(nameof(Get), new { id = created.CuentaId }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var cuenta = await _repo.GetCuentaByIdAsync(id);
            if (cuenta == null) return NotFound();
            return Ok(cuenta);
        }

        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetByCliente(int clienteId)
        {
            var cuentas = await _repo.GetCuentasByClienteAsync(clienteId);
            // always return a list (may be empty)
            return Ok(cuentas);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cuenta cuenta)
        {
            if (id <= 0) return BadRequest();

            cuenta.CuentaId = id;

            var updated = await _repo.UpdateCuentaAsync(cuenta);
            if (updated == null) return NotFound();
            return Ok(updated);
        }
    }
}
