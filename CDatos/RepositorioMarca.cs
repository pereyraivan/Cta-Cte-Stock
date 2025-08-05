using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioMarca
    {
        private string respuesta = "";

        public List<Marca> ListarMarcas()
        {
            List<Marca> marcas = new List<Marca>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    marcas = db.Marca.ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return marcas;
        }

        public void RegistrarMarca(Marca marca)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    db.Marca.Add(marca);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void ModificarMarca(Marca marca)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var editarMarca = db.Marca.FirstOrDefault(x => x.IdMarca == marca.IdMarca);
                    if (editarMarca != null)
                    {
                        editarMarca.NombreMarca = marca.NombreMarca;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void EliminarMarca(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var marca = db.Marca.FirstOrDefault(x => x.IdMarca == id);
                    if (marca != null)
                    {
                        db.Marca.Remove(marca);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public string ObtenerRespuesta()
        {
            return respuesta;
        }
    }
}
