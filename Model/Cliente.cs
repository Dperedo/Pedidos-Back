using System;
using System.ComponentModel.DataAnnotations;

namespace Pedidos_back.Model
{
    public class Cliente : IModel
    {
        public Cliente() {
            this.FechaDeCreacion = DateTime.UtcNow;
        }
        [Key]
        public Guid Id { set; get; }
        [StringLength(20)]
        public string RUT { set; get; }
        [StringLength(20)]
        public string RazonSocial { set; get; }

        public DateTime FechaDeCreacion { set; get; }

        public Boolean Vigente { set; get; }
        

    }
}