using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorPago
    {
        private RepositorioPago _repositorioPago = new RepositorioPago();

        public string RegistrarPago(int cuotaId, decimal monto, DateTime fechaPago)
        {
            var cuota = _repositorioPago.ObtenerCuotaPorId(cuotaId);

            if (cuota == null)
                return "La cuota no existe.";

            if (cuota.Estado == "Pagada")
                return "La cuota ya está pagada.";

            // Crear y registrar el pago
            var pago = new Pago
            {
                VentaId = cuota.VentaId,
                Monto = monto,
                FechaDePago = fechaPago
            };

            string respuesta = _repositorioPago.RegistrarPago(pago);

            if (!string.IsNullOrEmpty(respuesta))
                return respuesta; // Error al registrar el pago

            // Actualizar estado de la cuota
            if (monto >= cuota.MontoCuota)
            {
                cuota.Estado = "Pagada";
            }
            else
            {
                cuota.Estado = "Pendiente";
            }

            _repositorioPago.ActualizarCuota(cuota);

            return "Pago registrado exitosamente.";
        }

        public List<Cuota> ObtenerCuotasPorVenta(int ventaId)
        {
            return _repositorioPago.ObtenerCuotasPorVenta(ventaId);
        }
    }
}
