﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSysProducto.DAL;
using MCSysProducto.EN;

namespace MCSysProducto.BL
{
    public class ProductoBL
    {
        private readonly ProductoDAL _ProductoDAL;
        public ProductoBL(ProductoDAL productoDAL)
        {
            _ProductoDAL = productoDAL;
        }

        public async Task<Producto> ObtenerPorIdAsync( Producto pProducto)
        {
            return await _ProductoDAL.ObtenerPorIdAsync(pProducto);
        }
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _ProductoDAL.ObtenerTodosAsync();
        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            return await _ProductoDAL.CrearAsync(pProducto);
        }
        public async Task<int> ModificarAsync(Producto pProducto)
        {
            return await _ProductoDAL.ModificarAsync(pProducto);
        }
        public async Task<int> EliminararAsync(Producto pProducto)
        {
            return await _ProductoDAL.EliminarAsync(pProducto);
        }
        public Task AgregarTodosAsync(List<Producto> pProductos)
        {
            return _ProductoDAL.AgregarTodosAsync(pProductos);
        }
    }
}
