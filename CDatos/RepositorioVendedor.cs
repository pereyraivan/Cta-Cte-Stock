using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CDatos
{
    public class RepositorioVendedor
    {
        private string respuesta = "";

        public void Guardar(Vendedor vendedor)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    db.Vendedor.Add(vendedor);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }

        public void Modificar(Vendedor vendedor)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    var editarVendedor = db.Vendedor.FirstOrDefault(x => x.VendedorId == vendedor.VendedorId);
                    if (editarVendedor != null)
                    {
                        editarVendedor.NombreYApellido = vendedor.NombreYApellido;
                        editarVendedor.Telefono = vendedor.Telefono;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    var vendedor = db.Vendedor.FirstOrDefault(x => x.VendedorId == id);
                    if (vendedor != null)
                    {
                        db.Vendedor.Remove(vendedor);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
        }

        public List<Vendedor> Listar()
        {
            List<Vendedor> vendedores = null;
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    vendedores = db.Vendedor.ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return vendedores;
        }
        public List<Vendedor> Buscar(string nombre)
        {
            List<Vendedor> vendedor = new List<Vendedor>();
            try
            {
                using (VentasCredimaxEntities db = new VentasCredimaxEntities())
                {
                    //clientes = db.SP_listaClientes().Where(x => x.NombreCompleto.Contains(nombre)).ToList();

                    vendedor = db.Vendedor.AsQueryable().Where(x => x.NombreYApellido.ToLower().Contains(nombre)).ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }

            return vendedor;
        }
    }
}
