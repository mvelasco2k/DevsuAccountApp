using Microsoft.EntityFrameworkCore;
using Microservice.Clients.Data;
using Microservice.Clients.Entities;

namespace Microservice.Clients.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _db;
        public ClienteRepository(AppDbContext db) { _db = db; }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Clientes.Include(c => c.Persona).FirstOrDefaultAsync(c => c.ClienteId == id);
            if (existing == null) return false;

            // Remove associated Persona if any
            if (existing.Persona != null)
            {
                _db.Personas.Remove(existing.Persona);
            }

            _db.Clientes.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _db.Clientes.Include(c => c.Persona).ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _db.Clientes.Include(c => c.Persona).FirstOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<Cliente?> UpdateAsync(Cliente cliente)
        {
            var existing = await _db.Clientes.Include(c => c.Persona).FirstOrDefaultAsync(c => c.ClienteId == cliente.ClienteId);
            if (existing == null) return null;
            UpdateClienteFields(existing, cliente);
            await HandlePersonaUpdate(existing, cliente.Persona);
            await _db.SaveChangesAsync();
            return existing;
        }

        private static void UpdateClienteFields(Cliente existing, Cliente incoming)
        {
            if (incoming.Contrasena != null)
            {
                existing.Contrasena = incoming.Contrasena;
            }

            if (incoming.Estado != null)
            {
                existing.Estado = incoming.Estado;
            }
        }

        private async Task HandlePersonaUpdate(Cliente existing, Persona? incoming)
        {
            if (incoming == null) return;

            var found = await _db.Personas.FirstOrDefaultAsync(p => p.Identificacion == incoming.Identificacion);

            if (found != null)
            {
                var owner = await _db.Clientes.FirstOrDefaultAsync(c => c.PersonaId == found.PersonaId);
                if (owner != null && owner.ClienteId != existing.ClienteId)
                {
                    throw new InvalidOperationException("Persona is already associated with another Cliente");
                }

                // update the found persona fields
                found.Nombre = incoming.Nombre;
                found.Genero = incoming.Genero;
                found.Edad = incoming.Edad;
                found.Direccion = incoming.Direccion;
                found.Telefono = incoming.Telefono;

                existing.Persona = found;
                return;
            }

            if (existing.Persona == null)
            {
                existing.Persona = incoming;
                return;
            }

            existing.Persona.Nombre = incoming.Nombre;
            existing.Persona.Genero = incoming.Genero;
            existing.Persona.Edad = incoming.Edad;
            existing.Persona.Identificacion = incoming.Identificacion;
            existing.Persona.Direccion = incoming.Direccion;
            existing.Persona.Telefono = incoming.Telefono;
        }
    }
}