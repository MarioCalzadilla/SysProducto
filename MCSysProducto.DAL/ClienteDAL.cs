using MCSysProducto.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.DAL
{
    public class ClienteDAL
    {

        readonly SysProductoDBContext _dbContext;
        public ClienteDAL(SysProductoDBContext sysProductoDBContext)
        {
            _dbContext = sysProductoDBContext;
        }

        public async Task<int> CrearAsync(Cliente pCliente)
        {
            Cliente cliente = new Cliente()
            {
                Nombre = pCliente.Nombre,
                Dui = pCliente.Dui,
                Direccion = pCliente.Direccion,
                Telefono = pCliente.Telefono,
                Email = pCliente.Email,
            };
            _dbContext.Add(cliente);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                _dbContext.Clientes.Remove(cliente);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;

        }

        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                cliente.Nombre = pCliente.Nombre;
                cliente.Dui = pCliente.Dui;
                cliente.Direccion = pCliente.Direccion;
                cliente.Telefono = pCliente.Telefono;
                cliente.Email = pCliente.Email;

                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;

        }

        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                return new Cliente
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Dui = cliente.Dui,
                    Direccion = cliente.Direccion,
                    Telefono = cliente.Telefono,
                    Email = cliente.Email,
                };
            }
            else
                return new Cliente();
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await _dbContext.Clientes.ToListAsync();
            if (clientes != null && clientes.Count > 0)
            {
                var list = new List<Cliente>();
                clientes.ForEach(p => list.Add(new Cliente
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Dui = p.Dui,
                    Direccion = p.Direccion,
                    Telefono = p.Telefono,
                    Email = p.Email,
                }));
                return list;
            }
            else
                return new List<Cliente> { };
        }
        public async Task AgregarTodosAsync(List<Cliente> pCliente)
        {
            await _dbContext.Clientes.AddRangeAsync(pCliente);
            await _dbContext.SaveChangesAsync();
        }
    }
}
