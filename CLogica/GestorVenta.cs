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
        public List<VentaDTO> ListarVentasFormularioVentas(string textoBusqueda)
        {
            // Si no hay texto de búsqueda, listar todas las ventas ordenadas por fecha
            if (string.IsNullOrEmpty(textoBusqueda))
            {
                return _repositorioVenta.ListarVentas("Fecha", false).Where(x => x.FechaAnulacion == null).ToList();
            }
            else
            {
                // Si hay texto de búsqueda, devolver lista vacía porque la búsqueda debe ser específica (por cliente o artículo)
                return new List<VentaDTO>();
            }
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
            catch (InvalidOperationException)
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
        public List<VentaDTO> FiltrarVentasPorFrecPago(string frecuenciaPago)
        {
            return _repositorioVenta.FiltrarVentasPorFrecPago(frecuenciaPago);
        }
        public bool EsDemo()
        {
            return _repositorioVenta.EsDemo();
        }

        public decimal ObtenerTotalVentasPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorioVenta.ObtenerTotalVentasPorFecha(fechaDesde, fechaHasta);
        }
    }
}
