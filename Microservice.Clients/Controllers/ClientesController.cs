using Microsoft.AspNetCore.Mvc;
using Microservice.Clients.Entities;
using Microservice.Clients.Repositories;
using System.Text.Json;

namespace Microservice.Clients.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _repo;
        public ClientesController(IClienteRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            var created = await _repo.CreateAsync(cliente);
            return CreatedAtAction(nameof(Get), new { id = created.ClienteId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Cliente cliente)
        {
            if (id <= 0) return BadRequest();

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();

            cliente.ClienteId = id;

            try
            {
                var personaResult = await UpdatePersonaIfNeeded(id, cliente.Persona);
                var clienteResult = await UpdateClienteIfNeeded(id, cliente);

                var result = clienteResult ?? personaResult ?? existing;
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        private bool HasClienteChanges(Cliente cliente)
        {
            return cliente.Contrasena != null || cliente.Estado != null;
        }

        private async Task<Cliente?> UpdatePersonaIfNeeded(int id, Persona? persona)
        {
            if (persona == null) return null;
            var personaOnly = new Cliente { ClienteId = id, Persona = persona };
            return await _repo.UpdateAsync(personaOnly);
        }

        private async Task<Cliente?> UpdateClienteIfNeeded(int id, Cliente cliente)
        {
            if (!HasClienteChanges(cliente)) return null;
            var clienteOnly = new Cliente { ClienteId = id, Contrasena = cliente.Contrasena, Estado = cliente.Estado };
            return await _repo.UpdateAsync(clienteOnly);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
