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
        public  IActionResult Todos1(string texto, string order, int? take = 10, int? page = 1)
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
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderByDescending(x => x.FechaDeCreacion).Skip(skip)
                .Take(Convert.ToInt32(take));

            if (order == "FechaDeCreacionAcending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderBy(x => x.FechaDeCreacion).Skip(skip)
                .Take(Convert.ToInt32(take));}
                else if (order == "FechaDeCreacionDescending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderByDescending(x => x.FechaDeCreacion).Skip(skip)
                .Take(Convert.ToInt32(take));
                }

            if (order == "NombreAcending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderBy(x => x.Nombre).Skip(skip)
                .Take(Convert.ToInt32(take));}
                else if (order == "NombreDescending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderByDescending(x => x.Nombre).Skip(skip)
                .Take(Convert.ToInt32(take));
                }

            if (order == "PrecioAcending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderBy(x => x.Precio).Skip(skip)
                .Take(Convert.ToInt32(take));}
                else if (order == "PrecioDescending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderByDescending(x => x.Precio).Skip(skip)
                .Take(Convert.ToInt32(take));
                }

            if (order == "VigenteAcending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderBy(x => x.Vigente).Skip(skip)
                .Take(Convert.ToInt32(take));}
                else if (order == "VigenteDescending") {
                datos = repository.GetAll().Where(x => texto == null || 
                x.Codigo.ToLower().Contains(texto.ToLower()) || 
                x.Nombre.ToLower().Contains(texto.ToLower())
                ).OrderByDescending(x => x.Vigente).Skip(skip)
                .Take(Convert.ToInt32(take));
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
                list = datos};

            return Ok(result);
        }
        
    }
}
