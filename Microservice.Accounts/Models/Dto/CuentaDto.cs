namespace Microservice.Accounts.Models.Dto
{
    public class CuentaDto
    {
        public string TipoCuenta { get; set; }
        public string NumeroCuenta { get; set; }
        public decimal SaldoActual { get; set; }
        public List<MovimientoDto> Movimientos { get; set; }
    }
}
