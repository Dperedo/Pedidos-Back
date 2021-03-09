using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos_back.Model
{
    public class Pedido : IModel
    {

        public Pedido() {
            this.FechaDeCreacion = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { set; get; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Secuencial { set; get; }
        public Cliente Cliente { set; get; }
        public Estado Estado { set; get; }
        public float Total { set; get; }
        public List<DetallePedido> DetallePedidos { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        public string Observaciones { set; get; }
        

    }
}
