using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CEntidades.DTOs
{
    public class CuotaDTO
    {
        public int CuotaId { get; set; }                
        public int VentaID { get; set; }               
        public int NumeroDeCuota { get; set; }          
        public decimal MontoCuota { get; set; }         
        public string FechaProgramada { get; set; }     
        public string FechaPago { get; set; }           
        public string Estado { get; set; }
    }
}
