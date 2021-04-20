using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos_back.Model;
using Pedidos_back.Repository;

namespace Pedidos_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : BaseController<Pedido>
    {
        private IRepository<Pedido> repository;
        private IRepository<Cliente> repositoryCliente;
        private IRepository<Producto> repositoryProducto;
        private IRepository<Estado> repositoryEstado;
        private IRepository<DetallePedido> repositoryDetallePedido;

        public PedidosController(IRepository<Pedido> _repository, 
            IRepository<Cliente> _repositoryCliente,
            IRepository<Producto> _repositoryProducto,
            IRepository<DetallePedido> _repoDetallePedido,
            IRepository<Estado> _repositoryEstado) : base(_repository)
        {
            repository = _repository;
            repositoryCliente = _repositoryCliente;
            repositoryProducto = _repositoryProducto;
            repositoryDetallePedido = _repoDetallePedido;
            repositoryEstado = _repositoryEstado;
        }

        [HttpGet("query")]
        public  IActionResult Todos2(string texto, string order = "FechaDeCreacionAcending", int? take = 10, int? page = 1)
        {
            int skip = 0;

            if (!take.HasValue) take = 10;
            if (!page.HasValue) page = 1;
            if(string.IsNullOrWhiteSpace(texto)) texto = null;

            skip = (page.Value - 1) * take.Value;

            var datos = repository.GetAll().Include(c => c.Cliente).Include(e => e.Estado).Include(d => d.DetallePedidos).ThenInclude(p => p.Producto).Where(x => texto == null ||
                x.Cliente.RazonSocial.ToLower().Contains(texto.ToLower())
                );

            /*if (order == "FechaDeCreacionAcending")
            {
                datos = datos.OrderBy(x => x.FechaDeCreacion);
            }
            else if (order == "FechaDeCreacionDescending")
            {
                datos = datos.OrderByDescending(x => x.FechaDeCreacion);
            }

            if (order == "ClienteAcending")
            {
                datos = datos.OrderBy(x => x.Cliente.RazonSocial);
            }
            else if (order == "ClienteDescending")
            {
                datos = datos.OrderByDescending(x => x.Cliente.RazonSocial);
            }

            if (order == "TotalAcending")
            {
                datos = datos.OrderBy(x => x.Total);
            }
            else if (order == "TotalDescending")
            {
                datos = datos.OrderByDescending(x => x.Total);
            }

            if (order == "EstadoAcending")
            {
                datos = datos.OrderBy(x => x.Estado.EstadoPedido);
            }
            else if (order == "EstadoDescending")
            {
                datos = datos.OrderByDescending(x => x.Estado.EstadoPedido);
            }*/

            int count = repository.GetAll().Where(x => texto == null || 
                x.Cliente.RazonSocial.ToLower().Contains(texto.ToLower())
                ).Count();

            var result = new { total = count, 
                take = take, 
                page = page, 
                skip = skip, 
                numpages = Math.Ceiling(Convert.ToDecimal(count) / Convert.ToDecimal(take)),
                list = datos.Skip(skip)
                .Take(Convert.ToInt32(take))
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> MostrarSolo(Guid id)
        {
            var item = await repository.GetAll().Include(c => c.Cliente).Include(e => e.Estado).Include(d => d.DetallePedidos).ThenInclude(p => p.Producto).SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound("no encontrado");
            }

            return Ok(item);
        }

        [HttpPost]
        public override async Task<IActionResult> Insertar(Pedido entity)
        {
            var item = await repository.GetByIdAsync(entity.Id);

            if(item != null)
                return Conflict("Ya existe");

            var cliente = await repositoryCliente.GetByIdAsync(entity.Cliente.Id);
            if (cliente != null)
            {
                entity.Cliente = cliente;
            }
            else
                return BadRequest("Cliente no existe");

            var estado = await repositoryEstado.GetByIdAsync(entity.Estado.Id);
            if (estado != null)
            {
                entity.Estado = estado;
            }
            else
                return BadRequest("Estado no existe");

            foreach (DetallePedido detallePedido in entity.DetallePedidos)
            {
                detallePedido.Id = Guid.Empty;

                if (detallePedido.Producto != null)
                {
                    var producto = await repositoryProducto.GetByIdAsync(detallePedido.Producto.Id);
                    if (producto != null)
                    {
                        detallePedido.Producto = producto;
                    }
                    else
                        return BadRequest("Uno de los productos de Detalle del Pedido no existe");
                }
            }

            return Ok(await repository.InsertAsync(entity));
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Eliminar(Guid id)
        {
            

            var pedido =  repository.GetAll().Include(d => d.DetallePedidos).ThenInclude(p => p.Producto).FirstOrDefault(x => x.Id == id);
            if (pedido != null)
            {

                await repository.DeleteAsync(pedido.Id);

                for (int i = 0; i < pedido.DetallePedidos.Count; i++)
                {
                    await repositoryDetallePedido.DeleteAsync(pedido.DetallePedidos[i].Id);
                }

                //repository.Context.Remove(pedido);
                //for (int i=0;i<pedido.DetallePedidos.Count ;i++)
                //{
                //    repository.Context.Remove(pedido.DetallePedidos[i]);
                //}
            }
            
            await repository.Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Actualizar(Guid id, Pedido entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            var item = await repository.GetNoTrackedByIdAsync(id);
            if (item == null)
            {
                return NotFound("no encontrado");
            }

            var cliente = await repository.Context
                .Clientes
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == entity.Cliente.Id);

            //var cliente = await repositoryCliente.GetByIdAsync(entity.Cliente.Id);
            if (cliente != null)
            {
                entity.Cliente = cliente;
            }
            else
                return BadRequest("Cliente no existe");

            var estado = await repositoryEstado.GetByIdAsync(entity.Estado.Id);
            if (estado != null)
            {
                entity.Estado = estado;
            }
            else
                return BadRequest("Estado no existe");

            try
            {

                foreach (var pd in entity.DetallePedidos)
                {
                    if (pd.Producto == null)
                    {
                        return BadRequest("no especifica Producto");
                    }

                    var producto = await repositoryProducto.GetByIdAsync(pd.Producto.Id);
                    if (producto != null)
                    {
                        pd.Producto = producto;
                    }
                    else
                        return BadRequest("no existe Producto");

                }

                var pedido = repository.Context.Pedidos.Include(c => c.DetallePedidos)
                    .Include(x=> x.Estado)
                    .FirstOrDefault(g => g.Id == entity.Id);

                repository.Context.Entry(pedido).CurrentValues.SetValues(entity);
                pedido.Cliente = entity.Cliente;
                pedido.Estado = entity.Estado;

                var pedidoDetalles = pedido.DetallePedidos.ToList();
                foreach (var pedidoDetalle in pedidoDetalles)
                {
                    var detalle = entity.DetallePedidos.SingleOrDefault(i => i.Id == pedidoDetalle.Id);
                    if (detalle != null)
                        repository.Context.Entry(pedidoDetalle).CurrentValues.SetValues(detalle);
                    else
                        repository.Context.Remove(pedidoDetalle);
                }

                foreach (var detalle in entity.DetallePedidos)
                {
                    if (pedidoDetalles.All(i => i.Id != detalle.Id))
                    {
                        pedido.DetallePedidos.Add(detalle);
                    }
                }
                // context.Entry<Client>(client).Property(x => x.DateTimeCreated).IsModified = false;
                // db.Entry(model).Property(x => x.Token).State = PropertyState.Unmodified;
                
                // repository.Context.Entry(pedido).Property(x => x.Secuencial).State = propertystate.Unmodified
                // repository.Context.Entry(pedido).Property(x => x.Secuencial).IsModified = false;
                repository.Context.Update(pedido);
                repository.Context.Entry<Pedido>(pedido).Property(x => x.Secuencial).IsModified = false;
                repository.Context.SaveChanges();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


            return Ok();
        }

    }
}