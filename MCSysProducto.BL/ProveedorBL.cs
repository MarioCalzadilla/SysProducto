using MCSysProducto.DAL;
using MCSysProducto.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.BL
{
  public class ProveedorBL
    {
        private readonly ProveedorDAL _ProveedorDAL;
        public ProveedorBL(ProveedorDAL proveedorDAL)
        {
            _ProveedorDAL = proveedorDAL;
        }

        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            return await _ProveedorDAL.ObtenerPorIdAsync(pProveedor);
        }
        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await _ProveedorDAL.ObtenerTodosAsync();
        }
        public async Task<int> CrearAsync(Proveedor pProveedor)
        {
            return await _ProveedorDAL.CrearAsync(pProveedor);
        }
        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            return await _ProveedorDAL.ModificarAsync(pProveedor);
        }
        public async Task<int> EliminararAsync(Proveedor pProveedor)
        {
            return await _ProveedorDAL.EliminarAsync(pProveedor);
        }
        public Task AgregarTodosAsync(List<Proveedor> pProveedor)
        {
            return _ProveedorDAL.AgregarTodosAsync(pProveedor);
        }
    }
}
