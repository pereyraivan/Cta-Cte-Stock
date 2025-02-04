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
        public List<VentaDTO> ListarVentas(string criterioOrden)
        {
            return _repositorioVenta.ListarVentas(criterioOrden);
        }
        public List<VentaDTO> ListarVentasFormularioVentas(string criterioOrden)
        {
            return _repositorioVenta.ListarVentas(criterioOrden).Where(x => x.FechaAnulacion == null).ToList();
        }
        public List<VentaDTO> ListarVentasMenu(string criterioOrden)
        {
            return _repositorioVenta.ListarVentasMenu(criterioOrden);
        }
        public void ModificarVenta(Venta venta)
        {
            try
            {
                _repositorioVenta.ModificarVenta(venta);
            }
            catch (InvalidOperationException ex)
            {
                throw; 
            }
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
