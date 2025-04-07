using MCSysProducto.DAL;
using MCSysProducto.EN;
using MCSysProducto.EN.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.BL
{
    public class VentaBL
    {
        readonly VentaDAL _ventaDAL;

        public VentaBL(VentaDAL pVentaDAL)
        {
            _ventaDAL = pVentaDAL;
        }

        public async Task<int> CrearAsync(Venta pVenta)
        {
            return await _ventaDAL.CrearAsync(pVenta);
        }
        public async Task<int> AnularAsync(int idVenta)
        {
            return await _ventaDAL.AnularAsync(idVenta);
        }
        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            return await _ventaDAL.ObtenerPorIdAsync(idVenta);
        }
        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            return await _ventaDAL.ObtenerTodosAsync();
        }
        public async Task<List<Venta>> ObtenerPorEstadoAsync(byte estado)
        {
            return await _ventaDAL.ObtenerPorEstadoAsync(estado);
        }
        public async Task<List<Venta>> ObtenerReporteVentasAsync(VentaFiltros filtro)
        {
            return await _ventaDAL.ObtenerReporteVentasAsync(filtro);
        }


    }
}
