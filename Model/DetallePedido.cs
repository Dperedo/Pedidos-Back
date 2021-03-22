using System;
using System.ComponentModel.DataAnnotations;

namespace Pedidos_back.Model
{
    public class DetallePedido : IModel
    {
        public DetallePedido() {
            this.FechaDeCreacion = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { set; get; }
        public Producto Producto { set; get; }
        public int Cantidad { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        
    }
}

/*
    detalle pedido
        ID
        producto
        cantidad
*/