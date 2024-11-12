using CDatos;
using CEntidades;
using CEntidades.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLogica
{  
    public class GestorCliente
    {
        private RepositorioCliente _repositorioCliente = new RepositorioCliente();     
        public void Eliminar(int id)
        {
            _repositorioCliente.Eliminar(id);
        }
        public List<Cliente> FiltrarPorDocumento(string numero)
        {
            return _repositorioCliente.FiltrarPorDocumento(numero);
        }
        public List<Cliente> FiltrarPorNombre(string nombre)
        {
            return _repositorioCliente.FiltrarPorNombre(nombre);
        }
        public List<Cliente> FiltrarPorApellido(string apellido)
        {
            return _repositorioCliente.FiltrarPorApellido(apellido);
        }
        public void Guardar(Cliente cliente)
        {
            _repositorioCliente.Guardar(cliente);
        }
        public List<Cliente> Listar()
        {
            return _repositorioCliente.Listar();
        }
        public List<ClienteDTO> CargarComboCliente()
        {
            return _repositorioCliente.CargaComboCliente();
        }
        public void Modificar(Cliente cliente)
        {
            _repositorioCliente.Modificar(cliente);
        }
        public List<Cliente> ValidarExistencia(string nombre, string apellido, int numDoc)
        {
            return _repositorioCliente.ValidarExistencia(nombre, apellido, numDoc);
        }
    }
}
