using CDatos;
using CEntidades;
using System.Collections.Generic;

namespace CLogica
{
    public class GestorDiaDeSemana
    {
        private RepositorioDiaDeSemana _repositorio = new RepositorioDiaDeSemana();
        public List<DiaDeSemana> ObtenerDiasDeSemana()
        {
            return _repositorio.ObtenerDiasDeSemana();
        }
    }
}
