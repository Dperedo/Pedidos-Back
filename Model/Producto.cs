using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Pedidos_back.Model
{
    public class Producto : IModel
    {
        
        [Key]
        public Guid Id { get; set; }
        [StringLength(20)]
        public string Codigo { get; set; }
        [StringLength(200)]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public DateTime FechaDeCreacion { set; get; }
        public Boolean Vigente { set; get; }

    }
}
