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
    public class ClientesController : BaseController<Cliente>
    {
        private IRepository<Cliente> repository;
        protected ContenerContext context;

        public ClientesController(ContenerContext _context, IRepository<Cliente> _repository) : base(_repository)
        {
            repository = _repository;
            this.context = _context;
        }

        [HttpGet("query")]
        [Authorize]
        
        public  IActionResult Todos1(string texto, string order = "FechaDeCreacionAcending", int? take = 10, int? page = 1)
        {
            int skip = 0;

            if (!take.HasValue) take = 10;
            if (!page.HasValue) page = 1;
            if(string.IsNullOrWhiteSpace(texto)) texto = null;

            //if ( string.IsNullOrWhiteSpace(page) || !int.TryParse(page, out _)) page = "1";
            //if ( string.IsNullOrWhiteSpace(take) || !int.TryParse(take, out _)) take = "10";

            //if (Int32.Parse(page) > 0) skip = (Int32.Parse(page) - 1) * Convert.ToInt32(take);

            skip = (page.Value - 1) * take.Value;

            var datos = repository.GetAll().Where(x => texto == null ||
                x.RUT.ToLower().Contains(texto.ToLower()) ||
                x.RazonSocial.ToLower().Contains(texto.ToLower())
                );

            if (order == "FechaDeCreacionAcending")
            {
                datos = datos.OrderBy(x => x.FechaDeCreacion);
            }
            else if (order == "FechaDeCreacionDescending")
            {
                datos = datos.OrderByDescending(x => x.FechaDeCreacion);
            }

            if (order == "RazonSocialAcending")
            {
                datos = datos.OrderBy(x => x.RazonSocial);
            }
            else if (order == "RazonSocialDescending")
            {
                datos = datos.OrderByDescending(x => x.RazonSocial);
            }

            if (order == "RUTAcending")
            {
                datos = datos.OrderBy(x => x.RUT);
            }
            else if (order == "RUTDescending")
            {
                datos = datos.OrderByDescending(x => x.RUT);
            }

            if (order == "VigenteAcending")
            {
                datos = datos.OrderBy(x => x.Vigente);
            }
            else if (order == "VigenteDescending")
            {
                datos = datos.OrderByDescending(x => x.Vigente);
            }

            /*if (vigente == "true") 
            {
                datos = repository.GetAll().Where(x => x.Vigente == true);
            }*/

            int count = repository.GetAll().Where(x => texto == null || 
                x.RUT.ToLower().Contains(texto.ToLower()) || 
                x.RazonSocial.ToLower().Contains(texto.ToLower())
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
            // response = Ok(new {token = token}); x.RUT == texto
        }

        [HttpGet("vigente")]
        public virtual IActionResult TodosVigente()
        {
            return Ok(repository.GetAll().Where(x => x.Vigente == true));
        }

        [HttpPost]
        public override async Task<IActionResult> Insertar(Cliente entity)
        {
            var item = await repository.GetNoTrackedByIdAsync(entity.Id);
            if (item != null)
            {
                if (entity.Id == item.Id)
                {
                    return Conflict("Ya existe Id");
                } 
                else if (entity.RUT == item.RUT)
                {
                    return Conflict("Ya existe este rut");
                }
            }
            return Ok(await repository.InsertAsync(entity));
        }

        [HttpPut("{id}")]
        public override async Task<IActionResult> Actualizar(Guid id, Cliente entity)
        {
            if(id != entity.Id)
            {
                return BadRequest();
            }

            //var item = await repository.GetNoTrackedByIdAsync(id);
            var existe = await repository.GetAll().AsNoTracking().AnyAsync(x => x.Id == entity.Id);
            if (!existe)
            {
                return NotFound("No encontrado");
            }

            //var info = repository.GetAll().AsNoTracking().Where(x => x.Id != entity.Id && x.RUT == entity.RUT).SingleOrDefaultAsync();
            var info = await repository.GetAll().AsNoTracking().AnyAsync(x => x.Id != entity.Id && x.RUT == entity.RUT);
            if(info)
            {
                return BadRequest();
            }

            await repository.UpdateAsync(entity);
            return Ok();
        }

    }
}
