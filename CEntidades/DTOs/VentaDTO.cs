using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEntidades.DTOs
{
    public class VentaDTO
    {
        public int VentaId { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } // Nombre en lugar de ClientId
        public string Articulo { get; set; }
        public int? Talle { get; set; }
        public string FormaDePago { get; set; } // Nombre en lugar de FormaDePagoId
        public decimal? Precio { get; set; }
        public int? Cuotas { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeCancelacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }

    }
}
