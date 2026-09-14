using Microservice.Accounts.Entities;
using Microservice.Accounts.Models.Dto;

namespace Microservice.Accounts.Repositories
{
    public interface IAccountRepository
    {
        Task<Cuenta> CreateCuentaAsync(Cuenta cuenta);
        Task<Cuenta?> GetCuentaByIdAsync(int id);
        Task<IEnumerable<Cuenta>> GetCuentasByClienteAsync(int clienteId, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<Cuenta?> UpdateCuentaAsync(Cuenta cuenta);
        Task<Movimiento> CreateMovimientoAsync(Movimiento movimiento);
        Task<IEnumerable<Movimiento>> GetMovimientosByCuentaAsync(int cuentaId, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        // Movement update removed: modifications to existing movements are not supported
    }
}