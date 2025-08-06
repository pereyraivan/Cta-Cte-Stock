using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEntidades;
using CDatos;
using CLogica;

namespace CLogica
{
    public class GestorAireAcondicionado
    {
        private RepositorioAireAcondicionado repositorio;

        public GestorAireAcondicionado()
        {
            repositorio = new RepositorioAireAcondicionado();
        }

        public string GetUltimoError()
        {
            return repositorio.GetUltimoError();
        }

        public bool GuardarTrabajo(CEntidades.TrabajoAireAcondicionado trabajo)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrEmpty(trabajo.DescripcionTrabajo))
                {
                    throw new Exception("La descripción del trabajo es obligatoria");
                }

                if (trabajo.Precio.HasValue && trabajo.Precio.Value <= 0)
                {
                    throw new Exception("El precio debe ser mayor a cero");
                }

                // Validar que no exista un trabajo con la misma descripción
                if (repositorio.ExisteDescripcion(trabajo.DescripcionTrabajo, trabajo.IdTrabajo == 0 ? (int?)null : trabajo.IdTrabajo))
                {
                    throw new Exception("Ya existe un trabajo con esa descripción");
                }

                repositorio.Guardar(trabajo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ModificarTrabajo(CEntidades.TrabajoAireAcondicionado trabajo)
        {
            try
            {
                // Validaciones
                if (trabajo.IdTrabajo <= 0)
                {
                    throw new Exception("Debe especificar un ID de trabajo válido");
                }

                if (string.IsNullOrEmpty(trabajo.DescripcionTrabajo))
                {
                    throw new Exception("La descripción del trabajo es obligatoria");
                }

                if (trabajo.Precio.HasValue && trabajo.Precio.Value <= 0)
                {
                    throw new Exception("El precio debe ser mayor a cero");
                }

                // Validar que no exista otro trabajo con la misma descripción
                if (repositorio.ExisteDescripcion(trabajo.DescripcionTrabajo, trabajo.IdTrabajo))
                {
                    throw new Exception("Ya existe otro trabajo con esa descripción");
                }

                repositorio.Modificar(trabajo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EliminarTrabajo(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new Exception("Debe especificar un ID de trabajo válido");
                }

                repositorio.Eliminar(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<CEntidades.TrabajoAireAcondicionado> ListarTrabajos()
        {
            try
            {
                return repositorio.Listar();
            }
            catch (Exception)
            {
                return new List<CEntidades.TrabajoAireAcondicionado>();
            }
        }

        public List<CEntidades.TrabajoAireAcondicionado> BuscarTrabajosPorDescripcion(string descripcion)
        {
            try
            {
                if (string.IsNullOrEmpty(descripcion))
                {
                    return ListarTrabajos();
                }

                return repositorio.BuscarPorDescripcion(descripcion);
            }
            catch (Exception)
            {
                return new List<CEntidades.TrabajoAireAcondicionado>();
            }
        }

        public CEntidades.TrabajoAireAcondicionado ObtenerTrabajoPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return null;
                }

                return repositorio.ObtenerPorId(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ExisteDescripcion(string descripcion, int? trabajoIdExcluir = null)
        {
            try
            {
                return repositorio.ExisteDescripcion(descripcion, trabajoIdExcluir);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
