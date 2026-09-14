namespace Microservice.Accounts.Entities
{
    public class Movimiento
    {
        public int MovimientoId { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }
        public DateTime Fecha { get; set; }
        public int CuentaId { get; set; }
        public string? TipoMovimiento { get; set; }
        public Cuenta? Cuenta { get; set; }
    }
}