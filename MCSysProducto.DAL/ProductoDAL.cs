using MCSysProducto.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.DAL
{
    public class ProductoDAL
    {
         
        readonly SysProductoDBContext _dbContext;
        public ProductoDAL(SysProductoDBContext context)
        {
            _dbContext = context;
        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            Producto proucto = new Producto()
            {
                Nombre = pProducto.Nombre,
                Precio = pProducto.Precio,
                CantidadDisponible = pProducto.CantidadDisponible,
                FechaCreacion = pProducto.FechaCreacion,
            };
            _dbContext.Add(proucto);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(Producto pProducto)
        {
            var rol = _dbContext.Productos.FirstOrDefault(s => s.Id == pProducto.Id);
            if (rol != null)
            {
                _dbContext.Productos.Remove(rol);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;

        }

        public async Task<int> ModificarAsync(Producto pProducto)
        {
            var producto = await _dbContext.Productos.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null)
            {
                producto.Nombre = pProducto.Nombre;
                producto.Precio = pProducto.Precio;
                producto.CantidadDisponible = pProducto.CantidadDisponible;
                producto.FechaCreacion = pProducto.FechaCreacion;
                _dbContext.Productos.Update(producto);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;

        }
        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            var prodcuto = await _dbContext.Productos.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (prodcuto != null && prodcuto.Id != 0)
            {
                return new Producto
                {
                    Id = prodcuto.Id,
                    Nombre = prodcuto.Nombre,
                    Precio = prodcuto.Precio,
                    CantidadDisponible = prodcuto.CantidadDisponible,
                    FechaCreacion = prodcuto.FechaCreacion,
                };
            }
            else
                return new Producto();
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            var productos = await _dbContext.Productos.ToListAsync();
            if (productos != null && productos.Count > 0)
            {
                var list = new List<Producto>();
                productos.ForEach(s => list.Add(new Producto
                {
                    Id = s.Id,
                    Nombre = s.Nombre, 
                    Precio = s.Precio, 
                    FechaCreacion=s.FechaCreacion, 
                    CantidadDisponible=s.CantidadDisponible,
                }));
                return list;
            }
            else
                return new List<Producto>();
        }
        public async Task AgregarTodosAsync(List<Producto> pProducto) 
        {
            await _dbContext.Productos.AddRangeAsync(pProducto);
            await _dbContext.SaveChangesAsync();
        }

    }
}
