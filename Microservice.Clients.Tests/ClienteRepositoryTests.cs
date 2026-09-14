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

            var cliente = new Cliente { Nombre = "Juan", Email = "juan@example.com", Persona = new Persona { Cedula = "123" } };
            var created = await repo.CreateAsync(cliente);

            var fetched = await repo.GetByIdAsync(created.Id);

            Assert.NotNull(fetched);
            Assert.Equal("Juan", fetched!.Nombre);
            Assert.Equal("123", fetched.Persona!.Cedula);
        }
    }
}
