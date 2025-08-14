using System;

namespace CEntidades.DTOs
{
    public class MovimientoDTO
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } // "Ingreso" o "Egreso"
        public string Concepto { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Cliente { get; set; }
        public int? VentaId { get; set; }
        public int? CuotaId { get; set; }
    }
}
