using System;
using System.ComponentModel.DataAnnotations;

namespace Pedidos_back.Model
{
    public class Estado : IModel
    {
        public Guid Id { set; get; } // int
        [StringLength(20)]
        public string EstadoPedido { set; get; }
        

    }
}