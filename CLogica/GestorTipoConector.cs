using CDatos;
using CEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{
    public class GestorTipoConector
    {
        private RepositorioTipoConector _repositorioTipoConector = new RepositorioTipoConector();

        public List<TipoConector> ListarTipoConectores()
        {
            return _repositorioTipoConector.ListarTipoConectores();
        }

        public void RegistrarTipoConector(TipoConector tipoConector)
        {
            _repositorioTipoConector.RegistrarTipoConector(tipoConector);
        }

        public void ModificarTipoConector(TipoConector tipoConector)
        {
            _repositorioTipoConector.ModificarTipoConector(tipoConector);
        }

        public void EliminarTipoConector(int id)
        {
            _repositorioTipoConector.EliminarTipoConector(id);
        }

        public string ObtenerRespuesta()
        {
            return _repositorioTipoConector.ObtenerRespuesta();
        }
    }
}
