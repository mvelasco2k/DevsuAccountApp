using Microsoft.AspNetCore.Mvc;
using Microservice.Accounts.Repositories;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Models.Dto;
using Microservice.Clients.Repositories;
using System.Linq;

namespace Microservice.Accounts.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IClienteRepository _clientRepo;

        public ReportesController(IAccountRepository accountRepo, IClienteRepository clientRepo)
        {
            _accountRepo = accountRepo;
            _clientRepo = clientRepo;
        }

        [HttpGet("estado-cuenta")]
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

            var clienteInfo = await _clientRepo.GetByIdAsync(cliente);
            var cuentas = await _accountRepo.GetCuentasByClienteAsync(cliente, start, end);

            var cuentasDto = cuentas.Select(c => new CuentaDto
            {
                TipoCuenta = c.TipoCuenta,
                NumeroCuenta = c.NumeroCuenta,
                Movimientos = (c.Movimientos != null ? c.Movimientos.Select(m => new MovimientoDto
                {
                    Valor = m.Valor,
                    Saldo = m.Saldo,
                    TipoMovimiento = m.TipoMovimiento,
                    Fecha = m.Fecha
                }).ToList() : new List<MovimientoDto>()),
                SaldoActual = (c.Movimientos != null && c.Movimientos.Any() ? c.Movimientos.OrderBy(m => m.Fecha).Last().Saldo : c.SaldoInicial)
            }).ToList();

            var transaccion = new TransaccionDto
            {
                NombreCliente = clienteInfo?.Persona?.Nombre ?? string.Empty,
                Cuentas = cuentasDto
            };

            return Ok(new {cuentas = transaccion });
        }
    }
}
