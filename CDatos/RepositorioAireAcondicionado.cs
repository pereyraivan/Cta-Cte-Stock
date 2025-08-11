using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEntidades;

namespace CDatos
{
    public class RepositorioAireAcondicionado
    {
        private string respuesta = "";

        public string GetUltimoError()
        {
            return respuesta;
        }

        public void Guardar(CEntidades.TrabajoAireAcondicionado trabajo)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    db.TrabajoAireAcondicionado.Add(trabajo);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al guardar el trabajo de aire acondicionado: {e.Message}", e);
            }
        }

        public void Modificar(CEntidades.TrabajoAireAcondicionado trabajo)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var trabajoExistente = db.TrabajoAireAcondicionado.FirstOrDefault(x => x.IdTrabajo == trabajo.IdTrabajo);
                    if (trabajoExistente != null)
                    {
                        trabajoExistente.DescripcionTrabajo = trabajo.DescripcionTrabajo;
                        trabajoExistente.Cliente = trabajo.Cliente;
                        trabajoExistente.Precio = trabajo.Precio;
                        trabajoExistente.FechaTrabajo = trabajo.FechaTrabajo;
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("El trabajo de aire acondicionado no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al modificar el trabajo de aire acondicionado: {e.Message}", e);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var trabajo = db.TrabajoAireAcondicionado.FirstOrDefault(x => x.IdTrabajo == id);
                    if (trabajo != null)
                    {
                        db.TrabajoAireAcondicionado.Remove(trabajo);
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("El trabajo de aire acondicionado no existe en la base de datos");
                    }
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                throw new Exception($"Error al eliminar el trabajo de aire acondicionado: {e.Message}", e);
            }
        }

        public List<CEntidades.TrabajoAireAcondicionado> Listar()
        {
            List<CEntidades.TrabajoAireAcondicionado> trabajos = new List<CEntidades.TrabajoAireAcondicionado>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    trabajos = db.TrabajoAireAcondicionado.ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return trabajos;
        }

        public List<CEntidades.TrabajoAireAcondicionado> BuscarPorDescripcion(string descripcion)
        {
            List<CEntidades.TrabajoAireAcondicionado> trabajos = new List<CEntidades.TrabajoAireAcondicionado>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    trabajos = db.TrabajoAireAcondicionado
                        .Where(x => x.DescripcionTrabajo.Contains(descripcion))
                        .ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return trabajos;
        }

        public CEntidades.TrabajoAireAcondicionado ObtenerPorId(int id)
        {
            CEntidades.TrabajoAireAcondicionado trabajo = null;
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    trabajo = db.TrabajoAireAcondicionado.FirstOrDefault(x => x.IdTrabajo == id);
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return trabajo;
        }

        public bool ExisteDescripcion(string descripcion, int? trabajoIdExcluir = null)
        {
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    var query = db.TrabajoAireAcondicionado.Where(x => x.DescripcionTrabajo == descripcion);
                    
                    if (trabajoIdExcluir.HasValue)
                    {
                        query = query.Where(x => x.IdTrabajo != trabajoIdExcluir.Value);
                    }
                    
                    return query.Any();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
                return false;
            }
        }

        public List<CEntidades.TrabajoAireAcondicionado> ListarPorFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<CEntidades.TrabajoAireAcondicionado> trabajos = new List<CEntidades.TrabajoAireAcondicionado>();
            try
            {
                using (ventas_cta_cteEntities db = new ventas_cta_cteEntities())
                {
                    trabajos = db.TrabajoAireAcondicionado
                        .Where(x => x.FechaTrabajo.HasValue && 
                                   x.FechaTrabajo.Value >= fechaDesde && 
                                   x.FechaTrabajo.Value <= fechaHasta)
                        .OrderByDescending(x => x.FechaTrabajo)
                        .ToList();
                }
            }
            catch (Exception e)
            {
                respuesta = e.Message;
            }
            return trabajos;
        }
    }
}
