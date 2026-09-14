using Microservice.Accounts.Entities;

namespace Microservice.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Cuenta> CreateCuentaAsync(Cuenta cuenta);
        Task<Cuenta?> GetCuentaByIdAsync(int id);
        Task<IEnumerable<Cuenta>> GetCuentasByClienteAsync(int clienteId);
        Task<Cuenta?> UpdateCuentaAsync(Cuenta cuenta);

        Task<Movimiento> CreateMovimientoAsync(Movimiento movimiento);
        Task<IEnumerable<Movimiento>> GetMovimientosByCuentaAsync(int cuentaId);
    }
}