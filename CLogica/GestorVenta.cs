using CDatos;
using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorVenta
    {

        private RepositorioVenta _repositorioVenta = new RepositorioVenta();
        public void RegistrarVenta(Venta venta)
        {
            _repositorioVenta.RegistrarVenta(venta);
        }
        public List<VentaDTO> ListarVentas()
        {
            return _repositorioVenta.ListarVentas();
        }
        public void ModificarVenta(Venta venta)
        {
            _repositorioVenta.ModificarVenta(venta);
        }
        public void EliminarVenta(int id)
        {
            _repositorioVenta.EliminarVenta(id);
        }
        public List <VentaDTO> FiltrarVentasPorCliente(string nombreApellidoCliente)
        {
            return _repositorioVenta.FiltrarVentasPorCliente(nombreApellidoCliente);
        }
        public List<VentaDTO> FiltrarVentasPorArticulo(string nombreArticulo)
        {
            return _repositorioVenta.FiltrarVentasPorArticulo(nombreArticulo);
        }
    }
}
