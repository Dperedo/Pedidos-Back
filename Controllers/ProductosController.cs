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
    public class ProductosController : BaseController<Producto>
    {
        private IRepository<Producto> repository;

        public ProductosController(IRepository<Producto> _repository) : base(_repository)
        {
            repository = _repository;
        }

        [HttpGet("query")]
        public  IActionResult Todos1(string texto, string order = "FechaDeCreacionAcending", int? take = 10, int? page = 1)
        {
            int skip = 0;

            if (!take.HasValue) take = 10;
            if (!page.HasValue) page = 1;
            if(string.IsNullOrWhiteSpace(texto)) texto = null;

            skip = (page.Value - 1) * take.Value;

            var datos = repository.GetAll().Where(x => texto == null ||
                x.Nombre.ToLower().Contains(texto.ToLower()) ||
                x.Codigo.ToLower().Contains(texto.ToLower())
                );

            if (order == "FechaDeCreacionAcending")
            {
                datos = datos.OrderBy(x => x.FechaDeCreacion);
            }
            else if (order == "FechaDeCreacionDescending")
            {
                datos = datos.OrderByDescending(x => x.FechaDeCreacion);
            }

            if (order == "CodigoAcending")
            {
                datos = datos.OrderBy(x => x.Codigo);
            }
            else if (order == "CodigoDescending")
            {
                datos = datos.OrderByDescending(x => x.Codigo);
            }

            if (order == "NombreAcending")
            {
                datos = datos.OrderBy(x => x.Nombre);
            }
            else if (order == "NombreDescending")
            {
                datos = datos.OrderByDescending(x => x.Nombre);
            }

            if (order == "PrecioAcending")
            {
                datos = datos.OrderBy(x => x.Precio);
            }
            else if (order == "PrecioDescending")
            {
                datos = datos.OrderByDescending(x => x.Precio);
            }

            if (order == "VigenteAcending")
            {
                datos = datos.OrderBy(x => x.Vigente);
            }
            else if (order == "VigenteDescending")
            {
                datos = datos.OrderByDescending(x => x.Vigente);
            }

            int count = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
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
        
    }
}
