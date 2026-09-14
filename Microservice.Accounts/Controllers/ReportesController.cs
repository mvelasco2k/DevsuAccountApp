using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Repositories;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Models.Dto;
using System.Linq;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IAccountRepository _repo;

        public ReportesController(IAccountRepository repo)
        {
            _repo = repo;
        }

        // GET /reportes?fecha=2023-01-01,2023-01-31&cliente=123
        [HttpGet]
        public async Task<IActionResult> EstadoCuenta([FromQuery] string fecha, [FromQuery] int cliente)
        {
            DateTime? start = null, end = null;
            if (!string.IsNullOrWhiteSpace(fecha))
            {
                var parts = fecha.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts.Length == 2 && DateTime.TryParse(parts[0], out var s) && DateTime.TryParse(parts[1], out var e))
                {
                    start = s.Date;
                    end = e.Date.AddDays(1).AddTicks(-1);
                }
            }

            var cuentas = await _repo.GetCuentasByClienteAsync(cliente, start, end);

            var result = cuentas.Select(c => new TransaccionDto { cuenta = c, movimientos = (c.Movimientos != null ? c.Movimientos.ToList() : new List<Movimiento>()) }).ToList();

            return Ok(new { cliente, fecha = fecha, cuentas = result });
        }
    }
}
