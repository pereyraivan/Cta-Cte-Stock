using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDatos
{
    public class RepositorioCliente
    {
        private string respuesta = "";
        public void Eliminar(int id)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    var cliente = db.Cliente.FirstOrDefault(x => x.ClientId == id);
                    cliente.FechaAnulacion = DateTime.Today;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                respuesta = e.Message;
            }
        }
        public List<Cliente> FiltrarPorDocumento(string numero)
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    clientes = db.Cliente.Where(x => x.DNI.ToString().StartsWith(numero) && x.FechaAnulacion == null).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }

            return clientes;
        }
        public List<Cliente> FiltrarPorNombre(string nombre)
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    //clientes = db.SP_listaClientes().Where(x => x.NombreCompleto.Contains(nombre)).ToList();

                    clientes = db.Cliente.AsQueryable().Where(x => x.Nombre.ToLower().Contains(nombre) && x.FechaAnulacion == null).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }

            return clientes;
        }
        public List<Cliente> FiltrarPorApellido(string apellido)
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    //clientes = db.SP_listaClientes().Where(x => x.NombreCompleto.Contains(nombre)).ToList();

                    clientes = db.Cliente.AsQueryable().Where(x => x.Apellido.ToLower().Contains(apellido) && x.FechaAnulacion == null).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }

            return clientes;
        }
        public void Guardar(Cliente cliente)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }

        }
        public List<Cliente> Listar()
        {
            List<Cliente> clientes = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    clientes = db.Cliente.Where(x => x.FechaAnulacion == null).ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return clientes;
        }
        public void Modificar(Cliente cliente)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    var editarCliente = db.Cliente.FirstOrDefault(x => x.ClientId == cliente.ClientId);
                    editarCliente.Nombre = cliente.Nombre;
                    editarCliente.Apellido = cliente.Apellido;
                    editarCliente.DNI = cliente.DNI;
                    editarCliente.Direccion = cliente.Direccion;
                    editarCliente.Telefono = cliente.Telefono;
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }
        public List<Cliente> ValidarExistencia(string nombre, string apellido, int numDoc)
        {
            List<Cliente> clientes = new List<Cliente>();
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                clientes = db.Cliente.AsQueryable().Where(x => x.Nombre.ToLower().Contains(nombre)
                           && x.Apellido.ToLower().Contains(apellido)
                           && x.DNI == numDoc
                           && x.FechaAnulacion == null).ToList();
            }
            return clientes;
        }
        public List<ClienteDTO> CargaComboCliente()
        {
            using (VentasCredimaxEntities db = new VentasCredimaxEntities())
            {
                var clientess = db.Cliente.
                    Where(x => x.FechaAnulacion == null)
                    .Select(c => new ClienteDTO
                    {
                        IdCliente = c.ClientId,
                        NombreCompleto = c.Apellido + ", " + c.Nombre + " (" + (c.DNI.HasValue ? c.DNI.ToString() : "Sin DNI") + ")",
                        DNI = c.DNI // Lo incluyes en caso de que lo necesites para otras operaciones
                    })
                    .ToList();
                return clientess;

            }
            
        }

    }
}
