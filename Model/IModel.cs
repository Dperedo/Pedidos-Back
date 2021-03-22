using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pedidos_back.Model
{
    public interface IModel
    {
        public Guid Id { get; set; }
    }
}