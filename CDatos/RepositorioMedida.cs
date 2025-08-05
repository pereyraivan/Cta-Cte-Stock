using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioMedida
    {
        private string respuesta = "";

        public List<Medida> ListarMedidas()
        {
            List<Medida> medidas = new List<Medida>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    medidas = db.Medida.ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return medidas;
        }

        public void RegistrarMedida(Medida medida)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    db.Medida.Add(medida);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void ModificarMedida(Medida medida)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var editarMedida = db.Medida.FirstOrDefault(x => x.IdMedida == medida.IdMedida);
                    if (editarMedida != null)
                    {
                        editarMedida.NombreMedida = medida.NombreMedida;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void EliminarMedida(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var medida = db.Medida.FirstOrDefault(x => x.IdMedida == id);
                    if (medida != null)
                    {
                        db.Medida.Remove(medida);
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
