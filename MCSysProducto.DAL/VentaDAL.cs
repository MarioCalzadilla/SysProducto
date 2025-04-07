using MCSysProducto.EN;
using MCSysProducto.EN.Filtros;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.DAL
{
    public class VentaDAL
    {
        readonly SysProductoDBContext _dbContext;

        public VentaDAL(SysProductoDBContext sysProductoDB)
        {
            _dbContext = sysProductoDB;
        }

        public async Task<int> CrearAsync(Venta pVenta)
        {
            _dbContext.Ventas.Add(pVenta);
            int result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                foreach (var detalle in pVenta.DetalleVentas)
                {
                    var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> AnularAsync(int idVenta)
        {
            var venta = await _dbContext.Ventas
                .Include(c => c.DetalleVentas)
                .FirstOrDefaultAsync(c => c.Id == idVenta);

            if (venta != null && venta.Estado != (byte)Venta.EnumEstadoVenta.Anulada)
            {
                 venta.Estado = (byte)Venta.EnumEstadoVenta.Anulada;

                foreach (var detalle in venta.DetalleVentas)
                {
                    var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible -= detalle.Cantidad;
                    }
                }
                return await _dbContext.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            var venta = await _dbContext.Ventas
                .Include(c => c.DetalleVentas).Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == idVenta);

            return venta ?? new Venta();
        }

        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            var ventas = await _dbContext.Ventas
                .Include(c => c.DetalleVentas)
                .Include(c => c.Cliente).ToListAsync();
            return ventas ?? new List<Venta>();
        }

        public async Task<List<Venta>> ObtenerPorEstadoAsync(byte estado)
        {
            var ventasQuery = _dbContext.Ventas.AsQueryable();

            if (estado != 0)
            {
                ventasQuery = ventasQuery.Where(c => c.Estado == estado);
            }
            else
            {
                ventasQuery = ventasQuery.Where(c => c.Estado != (byte)Venta.EnumEstadoVenta.Anulada);
            }

            ventasQuery = ventasQuery
                .Include(c => c.DetalleVentas)
                .Include(c => c.Cliente);

            var ventas = await ventasQuery.ToListAsync();

            return ventas ?? new List<Venta>();


        }

        public async Task<List<Venta>> ObtenerReporteVentasAsync(VentaFiltros filtro)
        {
            var ventasQuery = _dbContext.Ventas
            .Include(c => c.DetalleVentas)
            .ThenInclude(dc => dc.Producto)
            .Include(c => c.Cliente)
            .AsQueryable();

            if (filtro.FechaInicio.HasValue)
            {
                DateTime fechaInicio = filtro.FechaInicio.Value.Date; // Eliminar la hora, dejar solo la fecha
                ventasQuery = ventasQuery.Where(c => c.FechaVenta >= fechaInicio);
            }

            if (filtro.FechaFin.HasValue)
            {
                DateTime fechaFin = filtro.FechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir hasta el final del dia
                ventasQuery = ventasQuery.Where(c => c.FechaVenta <= fechaFin);
            }

            return await ventasQuery.ToListAsync();
        }



    }
}
