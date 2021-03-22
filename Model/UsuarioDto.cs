using System;
using System.ComponentModel.DataAnnotations;

namespace Pedidos_back.Model
{
    public class UsuarioDto
    {

        public Guid Id { get; set; }
        [StringLength(20)]
        public string Username { get; set; }
        [StringLength(20)]
        public string Nombre { get; set; }
        [StringLength(20)]
        public string RUT { get; set; }
        [StringLength(20)]
        public string Password { get; set; }

    }
}