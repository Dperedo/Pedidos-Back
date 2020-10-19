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
        public  IActionResult Todos1(string texto, int? take = 10, int? page = 1)
        {
            int skip = 0;

            if (!take.HasValue) take = 10;
            if (!page.HasValue) page = 1;

            //if ( string.IsNullOrWhiteSpace(page) || !int.TryParse(page, out _)) page = "1";
            //if ( string.IsNullOrWhiteSpace(take) || !int.TryParse(take, out _)) take = "10";

            //if (Int32.Parse(page) > 0) skip = (Int32.Parse(page) - 1) * Convert.ToInt32(take);

            skip = (page.Value - 1) * take.Value;

            int count = repository.GetAll().Count();

            var result = new { total = count, 
                take = take, 
                page = page, 
                skip = skip, 
                list = repository.GetAll().Where(x => texto == null || 
                x.RUT.ToLower().Contains(texto.ToLower()) || 
                x.RazonSocial.ToLower().Contains(texto.ToLower())
                ).Skip(skip)
                .Take(Convert.ToInt32(take))};

            return Ok(result);
            // response = Ok(new {token = token}); x.RUT == texto
        }

    }
}
