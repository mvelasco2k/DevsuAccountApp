namespace Microservice.Accounts.Entities
{
    public class Cuenta
    {
        public int CuentaId { get; set; }
        public string NumeroCuenta { get; set; } = string.Empty;
        public string TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public int ClienteId { get; set; }
        public ICollection<Movimiento>? Movimientos { get; set; }
    }
}