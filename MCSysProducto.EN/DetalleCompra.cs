using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSysProducto.EN
{
   public class DetalleCompra
    {
        [Key]
        public int Id { get; set; }

        public int IdCompra { get; set; }

        [Required(ErrorMessage = "El Producto es obligatorio")]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Column(TypeName = "decimal(10,2)")]
        public int PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Range(0.01, 999999.99, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal SubTotal { get; set; }   

        public virtual Compra? Compra { get; set; }

        public virtual Producto? Producto { get; set; }

    }
}
