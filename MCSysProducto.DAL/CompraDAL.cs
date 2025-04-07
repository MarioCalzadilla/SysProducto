using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSysProducto.EN;
using MCSysProducto.EN.Filtros;


namespace MCSysProducto.DAL
{
    public class CompraDAL
    {
        readonly SysProductoDBContext _dbContext;

        public CompraDAL(SysProductoDBContext sysProductoDB)
        {
            _dbContext = sysProductoDB;
        }

        public async Task<int> CrearAsync(Compra pCompra)
        {
            _dbContext.Compras.Add(pCompra);
            int result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                foreach (var detalle in pCompra.DetalleCompras)
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

        public async Task<int> AnularAsync(int idCompra)
        {
            var compra = await _dbContext.Compras
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            if (compra != null && compra.Estado != (byte)Compra.EnumEstadoCompra.Anulada)
            {
                compra.Estado = (byte)Compra.EnumEstadoCompra.Anulada;

                foreach (var detalle in compra.DetalleCompras)
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

        public async Task<Compra> ObtenerPorIdAsync(int idCompra)
        {
            var compra = await _dbContext.Compras
                .Include(c => c.DetalleCompras).Include(c => c.Proveedor)
                .FirstOrDefaultAsync(c => c.Id == idCompra);

            return compra ?? new Compra();
        }

        public async Task<List<Compra>> ObtenerTodosAsync()
        {
            var compras = await _dbContext.Compras
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor) .ToListAsync();
            return compras ?? new List<Compra>();
        }

        public async Task<List<Compra>> ObtenerPorEstadoAsync(byte estado)
        {
            var comprasQuery = _dbContext.Compras.AsQueryable();

            if (estado != 0)
            {
                comprasQuery = comprasQuery.Where(c => c.Estado == estado);
            }
            else
            {
                comprasQuery = comprasQuery.Where(c => c.Estado != (byte)Compra.EnumEstadoCompra.Anulada);
            }

            comprasQuery = comprasQuery
                .Include(c => c.DetalleCompras)
                .Include(c => c.Proveedor);

            var compras = await comprasQuery.ToListAsync();

            return compras ?? new List<Compra>();
        }


        public async Task<List<Compra>> ObtenerReporteComprasAsync(CompraFiltros filtro)
        {
            var comprasQuery = _dbContext.Compras
            .Include(c => c.DetalleCompras)
            .ThenInclude(dc => dc.Producto)
            .Include(c => c.Proveedor)
            .AsQueryable();

            if (filtro.FechaInicio.HasValue)
            {
                DateTime fechaInicio = filtro.FechaInicio.Value.Date; // Eliminar la hora, dejar solo la fecha
                comprasQuery = comprasQuery.Where(c => c.FechaCompra >= fechaInicio);
            }

            if (filtro.FechaFin.HasValue)
            {
                DateTime fechaFin = filtro.FechaFin.Value.Date.AddDays(1).AddSeconds(-1); // Incluir hasta el final del dia
                comprasQuery = comprasQuery.Where(c => c.FechaCompra <= fechaFin);
            }

            return await comprasQuery.ToListAsync();
        }

    } 

}