using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEntidades.DTOs
{
    public partial class ClienteDTO
    {
        public int IdCliente { get; set; }
        public string NombreCompleto { get; set; }
        public int? DNI { get; set; } 
        DateTime FechaAnulacion { get; set; }
    }
}
