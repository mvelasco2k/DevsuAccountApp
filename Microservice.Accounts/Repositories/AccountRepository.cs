using Microsoft.EntityFrameworkCore;
using Microservice.Accounts.Data;
using Microservice.Accounts.Entities;
using Microservice.Accounts.Exceptions;

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

            // apply movement: ensure sufficient balance for debits
            if (movimiento.Valor < 0)
            {
                // if account has no saldo inicial set (assume zero) or not enough funds -> error
                var current = cuenta.SaldoInicial;
                if (current + movimiento.Valor < 0)
                {
                    throw new InsufficientFundsException("Saldo no disponible");
                }
            }

            cuenta.SaldoInicial += movimiento.Valor;

            // set movement saldo after applying
            movimiento.Saldo = cuenta.SaldoInicial;
            movimiento.Fecha = DateTime.UtcNow;

            _db.Movimientos.Add(movimiento);

            await _db.SaveChangesAsync();
            return movimiento;
        }

        public async Task<Cuenta?> GetCuentaByIdAsync(int id)
        {
            return await _db.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.CuentaId == id);
        }

        public async Task<IEnumerable<Cuenta>> GetCuentasByClienteAsync(int clienteId)
        {
            return await _db.Cuentas.Where(c => c.ClienteId == clienteId).Include(c => c.Movimientos).ToListAsync();
        }

        public async Task<IEnumerable<Movimiento>> GetMovimientosByCuentaAsync(int cuentaId)
        {
            return await _db.Movimientos.Where(m => m.CuentaId == cuentaId).ToListAsync();
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
