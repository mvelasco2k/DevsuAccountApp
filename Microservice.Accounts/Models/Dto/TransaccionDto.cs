using Microservice.Accounts.Entities;

namespace Microservice.Accounts.Models.Dto
{
    public class TransaccionDto
    {
        public List<Movimiento> movimientos { get; set; }
        public Cuenta cuenta { get; set; }
    }
}
