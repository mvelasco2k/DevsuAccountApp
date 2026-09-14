using Microsoft.EntityFrameworkCore;
using Microservice.Accounts.Data;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Exceptions;
using Microservice.Accounts.Models.Dto;

namespace Microservice.Accounts.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _db;
        public AccountRepository(AppDbContext db) { _db = db; }

        public async Task<Cuenta> CreateCuentaAsync(Cuenta cuenta)
        {
            _db.Cuentas.Add(cuenta);
            await _db.SaveChangesAsync();
            return cuenta;
        }

        public async Task<IEnumerable<Movimiento>> GetMovimientosByCuentaAsync(int cuentaId, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _db.Movimientos.Where(m => m.CuentaId == cuentaId);
            if (fechaInicio.HasValue) query = query.Where(m => m.Fecha >= fechaInicio.Value);
            if (fechaFin.HasValue) query = query.Where(m => m.Fecha <= fechaFin.Value);

            return await query.OrderBy(m => m.Fecha).ToListAsync();
        }

        

        public async Task<Movimiento> CreateMovimientoAsync(Movimiento movimiento)
        {
            // validate account
            var cuenta = await _db.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == movimiento.CuentaId);
            if (cuenta == null) throw new KeyNotFoundException("Cuenta no encontrada");

            // determine tipo if not provided
            if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
            {
                movimiento.TipoMovimiento = movimiento.Valor >= 0 ? "CREDITO" : "DEBITO";
            }

            // compute current balance based on initial saldo plus existing movements
            var movimientosSum = await _db.Movimientos.Where(m => m.CuentaId == cuenta.CuentaId).SumAsync(m => (decimal?)m.Valor) ?? 0m;
            var currentBalance = cuenta.SaldoInicial + movimientosSum;

            // apply movement: ensure sufficient balance for debits
            if (movimiento.Valor < 0 && currentBalance + movimiento.Valor < 0)
            {
                throw new InsufficientFundsException("Saldo no disponible");
            }

            // set movement saldo after applying (do not modify cuenta.SaldoInicial)
            movimiento.Saldo = currentBalance + movimiento.Valor;
            movimiento.Fecha = DateTime.UtcNow;

            _db.Movimientos.Add(movimiento);

            await _db.SaveChangesAsync();
            return movimiento;
        }

        public async Task<Cuenta?> GetCuentaByIdAsync(int id)
        {
            return await _db.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.CuentaId == id);
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByClienteAsync(int clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var cuentas = await _db.Cuentas.Where(c => c.ClienteId == clienteId).ToListAsync();

            if (!fechaInicio.HasValue && !fechaFin.HasValue)
            {
                return await _db.Cuentas.Where(c => c.ClienteId == clienteId).Include(c => c.Movimientos).ToListAsync();
            }

            var result = new List<Cuenta>();
            foreach (var cuenta in cuentas)
            {
                var movimientosQuery = _db.Movimientos.Where(m => m.CuentaId == cuenta.CuentaId);
                if (fechaInicio.HasValue) movimientosQuery = movimientosQuery.Where(m => m.Fecha >= fechaInicio.Value);
                if (fechaFin.HasValue) movimientosQuery = movimientosQuery.Where(m => m.Fecha <= fechaFin.Value);

                cuenta.Movimientos = await movimientosQuery.OrderBy(m => m.Fecha).ToListAsync();
                result.Add(cuenta);
            }

            return result;
        }

        public async Task<Cuenta?> UpdateCuentaAsync(Cuenta cuenta)
        {
            var existing = await _db.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == cuenta.CuentaId);

            if (existing == null) return null;
            if (existing.ClienteId != cuenta.ClienteId) return null;

            existing.NumeroCuenta = cuenta.NumeroCuenta;
            existing.TipoCuenta = cuenta.TipoCuenta;
            existing.SaldoInicial = cuenta.SaldoInicial;
            existing.Estado = cuenta.Estado;
            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
