using Microservice.Accounts.Entities;

namespace Microservice.Accounts.Models.Dto
{
    public class TransaccionDto
    {
        public string NombreCliente { get; set; }
        public List<CuentaDto> Cuentas { get; set; }
    }
}
