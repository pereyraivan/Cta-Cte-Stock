using System;

namespace CEntidades.DTOs
{
    public class ReporteIngresoUnificado
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "Venta" o "Trabajo"
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? Precio { get; set; }
        public int? Cantidad { get; set; }
        public decimal Total { get; set; }
        public string FormaPago { get; set; }
        public string Estado { get; set; }
        
        // Propiedades específicas de ventas
        public int? DNI { get; set; }
        public string CodigoArticulo { get; set; }
        public int? Cuotas { get; set; }
        
        // Constructor por defecto
        public ReporteIngresoUnificado() { }
        
        // Constructor para crear desde venta
        public ReporteIngresoUnificado(sp_ReporteVentas_Result venta)
        {
            Id = venta.VentaId;
            Tipo = "Venta";
            Cliente = $"{venta.Nombre} {venta.Apellido}";
            Descripcion = venta.Articulo;
            Fecha = venta.FechaDeVenta;
            Precio = venta.Precio;
            Cantidad = venta.Cantidad;
            Total = venta.Total ?? 0;
            FormaPago = venta.FormaDePago;
            Estado = venta.Estado;
            DNI = venta.DNI;
            CodigoArticulo = venta.CodigoArticulo;
            Cuotas = venta.Cuotas;
        }
        
        // Constructor para crear desde trabajo
        public ReporteIngresoUnificado(TrabajoAireAcondicionado trabajo)
        {
            Id = trabajo.IdTrabajo;
            Tipo = "Trabajo A/A";
            Cliente = trabajo.Cliente ?? "N/A";
            Descripcion = trabajo.DescripcionTrabajo;
            Fecha = trabajo.FechaTrabajo;
            Precio = trabajo.Precio;
            Cantidad = 1;
            Total = trabajo.Precio ?? 0;
            FormaPago = "Contado"; // Asumimos que los trabajos son al contado
            Estado = "Completado";
        }
    }
}
