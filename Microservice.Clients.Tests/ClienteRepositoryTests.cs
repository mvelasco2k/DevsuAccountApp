using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservice.Clients.Data;
using Microservice.Clients.Entities;
using Microservice.Clients.Repositories;
using Xunit;

namespace Microservice.Clients.Tests
{
    public class ClienteRepositoryTests
    {
        [Fact]
        public async Task CreateAndGetById_Works()
        {
            var dbName = System.Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            await using var db = new AppDbContext(options);
            var repo = new ClienteRepository(db);

            var cliente = new Cliente
            {
                Contrasena = "pwd123",
                Estado = true,
                Persona = new Persona
                {
                    Nombre = "Juan",
                    Identificacion = "123",
                    Direccion = "Calle 1",
                    Telefono = "099999999"
                }
            };

            var created = await repo.CreateAsync(cliente);

            var fetched = await repo.GetByIdAsync(created.ClienteId);

            Assert.NotNull(fetched);
            Assert.Equal("pwd123", fetched!.Contrasena);
            Assert.Equal("123", fetched.Persona!.Identificacion);
        }
    }
}
