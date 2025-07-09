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
        public string NombreCliente { get; set; } 
        public string Articulo { get; set; }
        public int? Talle { get; set; }
        public string FormaDePago { get; set; } 
        public decimal? Precio { get; set; }
        public int? Cuotas { get; set; }
        public DateTime? FechaDeInicio { get; set; }
        public DateTime? FechaDeCancelacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public bool CuotasVencidas { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Total { get; set; }
        public string VendedorNombre { get; set; }
        public int? IdDiaSemana { get; set; } // Nuevo campo para filtrar por día de la semana
        public string DiaSemanaNombre { get; set; } // Nombre del día de la semana

    }
}
