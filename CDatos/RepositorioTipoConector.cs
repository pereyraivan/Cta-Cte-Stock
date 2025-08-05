using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDatos
{
    public class RepositorioTipoConector
    {
        private string respuesta = "";

        public List<TipoConector> ListarTipoConectores()
        {
            List<TipoConector> tipoConectores = new List<TipoConector>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    tipoConectores = db.TipoConector.ToList();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return tipoConectores;
        }

        public void RegistrarTipoConector(TipoConector tipoConector)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    db.TipoConector.Add(tipoConector);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void ModificarTipoConector(TipoConector tipoConector)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var editarTipoConector = db.TipoConector.FirstOrDefault(x => x.IdTipoConector == tipoConector.IdTipoConector);
                    if (editarTipoConector != null)
                    {
                        editarTipoConector.NombreTipoConector = tipoConector.NombreTipoConector;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
        }

        public void EliminarTipoConector(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var tipoConector = db.TipoConector.FirstOrDefault(x => x.IdTipoConector == id);
                    if (tipoConector != null)
                    {
                        db.TipoConector.Remove(tipoConector);
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
