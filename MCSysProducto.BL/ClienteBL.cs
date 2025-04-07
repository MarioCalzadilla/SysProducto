using MCSysProducto.DAL;
using MCSysProducto.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.BL
{
    public class ClienteBL
    {

        private readonly ClienteDAL _ClienteDAL;
        public ClienteBL(ClienteDAL clienteDAL)
        {
            _ClienteDAL = clienteDAL;
        }

        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            return await _ClienteDAL.ObtenerPorIdAsync(pCliente);
        }
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _ClienteDAL.ObtenerTodosAsync();
        }
        public async Task<int> CrearAsync(Cliente pCliente)
        {
            return await _ClienteDAL.CrearAsync(pCliente);
        }
        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            return await _ClienteDAL.ModificarAsync(pCliente);
        }
        public async Task<int> EliminararAsync(Cliente pCliente)
        {
            return await _ClienteDAL.EliminarAsync(pCliente);
        }
        public Task AgregarTodosAsync(List<Cliente> pCliente)
        {
            return _ClienteDAL.AgregarTodosAsync(pCliente);
        }
    }
}
