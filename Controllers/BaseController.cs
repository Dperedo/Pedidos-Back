using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pedidos_back.Model;
using Pedidos_back.Repository;

namespace Pedidos_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : class, IModel
    {
        private IRepository<T> repository;

        public BaseController(IRepository<T> _repository)
        {
            repository = _repository;
        }

        [HttpGet]
        public virtual IActionResult Todos(string take = "10", string skip = "0")
        {
            return Ok(repository.GetAll().Skip(Convert.ToInt32(skip)).Take(Convert.ToInt32(take)));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> MostrarSolo(Guid id)
        {
            var item = await repository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound("no encontrado");
            }
            return Ok(item);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Insertar(T entity)
        {
            var item = await repository.GetNoTrackedByIdAsync(entity.Id);
            if (item != null)
            {
                if (entity.Id == item.Id)
                {
                    return Conflict("ya existe");
                }
            }
            return Ok(await repository.InsertAsync(entity));
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Eliminar(Guid id)
        {
            var item = await repository.GetNoTrackedByIdAsync(id);
            if (item == null)
            {
                return NotFound("no encontrado");
            }
            await repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Actualizar(Guid id, T entity)
        {
            if(id != entity.Id)
            {
                return BadRequest();
            }
            var item = await repository.GetNoTrackedByIdAsync(id);
            if (item == null)
            {
                return NotFound("no encontrado");
            }
            await repository.UpdateAsync(entity);
            return Ok();
        }

    }
}
