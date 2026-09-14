using Microservice.Clients.Entities;

namespace Microservice.Clients.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<Cliente?> GetByIdAsync(int id);
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}