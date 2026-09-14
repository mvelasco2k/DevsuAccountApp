namespace Microservice.Accounts.Models.Dto
{
    public class MovimientoDto
    {
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public string TipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
    }
}
