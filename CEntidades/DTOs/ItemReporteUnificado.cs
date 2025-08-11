using System;

namespace CEntidades.DTOs
{
    public class ItemReporteUnificado
    {
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Descripcion { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public string FormaPago { get; set; }
        public string Estado { get; set; }
        
        // Constructor para ventas
        public ItemReporteUnificado(sp_ReporteVentas_Result venta)
        {
            Tipo = "Venta";
            Fecha = venta.FechaDeVenta;
            Cliente = $"{venta.Nombre} {venta.Apellido}";
            Descripcion = venta.Articulo;
            Cantidad = venta.Cantidad;
            PrecioUnitario = venta.Precio;
            Total = venta.Total ?? 0;
            FormaPago = venta.FormaDePago;
            Estado = venta.Estado;
        }
        
        // Constructor para trabajos
        public ItemReporteUnificado(TrabajoAireAcondicionado trabajo)
        {
            Tipo = "Trabajo A/A";
            Fecha = trabajo.FechaTrabajo ?? DateTime.Now;
            Cliente = trabajo.Cliente ?? "N/A";
            Descripcion = trabajo.DescripcionTrabajo;
            Cantidad = 1;
            PrecioUnitario = trabajo.Precio;
            Total = trabajo.Precio ?? 0;
            FormaPago = "Contado";
            Estado = "Completado";
        }
    }
}
