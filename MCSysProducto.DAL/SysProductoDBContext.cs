using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSysProducto.EN;
namespace MCSysProducto.DAL
{
    public class SysProductoDBContext :DbContext
    {
        public SysProductoDBContext(DbContextOptions<SysProductoDBContext> options) : base(options)
        {

        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Compra> Compras  { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleCompra>()
                 .HasOne(d => d.Compra)
                 .WithMany(c => c.DetalleCompras)
                 .HasForeignKey(d => d.IdCompra);

            base.OnModelCreating(modelBuilder);
                
        }

    }
}

