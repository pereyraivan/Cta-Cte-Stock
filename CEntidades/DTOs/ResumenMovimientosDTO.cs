namespace CEntidades.DTOs
{
    public class ResumenMovimientosDTO
    {
        public decimal TotalVentas { get; set; }
        public decimal TotalCuotas { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Diferencia { get; set; }
    }
}
