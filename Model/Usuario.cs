using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Pedidos_back.Model
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name ="Username")]
        [StringLength(20)]
        public string Username { get; set; }
        [StringLength(20)]
        public string Nombre { get; set; }
        [StringLength(20)]
        public string RUT { get; set; }
        
        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }
    }
}
