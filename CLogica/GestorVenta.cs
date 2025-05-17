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
        public List<VentaDTO> ListarVentas(string criterioOrden, bool mostrarTodas)
        {
            return _repositorioVenta.ListarVentas(criterioOrden, mostrarTodas);
        }
        public List<VentaDTO> ListarVentasFormularioVentas(string criterioOrden)
        {
            return _repositorioVenta.ListarVentas(criterioOrden, false).Where(x => x.FechaAnulacion == null).ToList();
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
        public List<VentaDTO> FiltrarVentasPorVendedor(string nombreVendedor)
        {
            return _repositorioVenta.FiltrarVentasPorVendedor(nombreVendedor);
        }
        public List<VentaDTO> FiltrarVentasPorFrecPago(string nombreVendedor)
        {
            return _repositorioVenta.FiltrarVentasPorFrecPago(nombreVendedor);
        }
        public bool EsDemo()
        {
            return _repositorioVenta.EsDemo();
        }
    }
}
