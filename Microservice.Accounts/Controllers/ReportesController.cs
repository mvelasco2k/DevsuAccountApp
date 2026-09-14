using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Repositories;
using Microservice.Accounts.Entities;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/reportes")]
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
            // parse fecha as two dates separated by comma
            DateTime? start = null, end = null;
            if (!string.IsNullOrWhiteSpace(fecha))
            {
                var parts = fecha.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts.Length == 2 && DateTime.TryParse(parts[0], out var s) && DateTime.TryParse(parts[1], out var e))
                {
                    start = s.Date;
                    end = e.Date.AddDays(1).AddTicks(-1); // include entire end day
                }
            }

            var cuentas = await _repo.GetCuentasByClienteAsync(cliente);

            var result = new List<object>();

            foreach (var cuenta in cuentas)
            {
                var movimientos = await _repo.GetMovimientosByCuentaAsync(cuenta.CuentaId);
                if (start.HasValue && end.HasValue)
                {
                    movimientos = movimientos.Where(m => m.Fecha >= start.Value && m.Fecha <= end.Value).ToList();
                }

                result.Add(new
                {
                    cuenta = new
                    {
                        cuenta.CuentaId,
                        cuenta.NumeroCuenta,
                        cuenta.TipoCuenta,
                        Saldo = cuenta.SaldoInicial
                    },
                    movimientos = movimientos.Select(m => new
                    {
                        m.MovimientoId,
                        m.Valor,
                        m.Saldo,
                        m.Fecha,
                        m.TipoMovimiento
                    })
                });
            }

            return Ok(new { cliente, fecha = fecha, cuentas = result });
        }
    }
}
