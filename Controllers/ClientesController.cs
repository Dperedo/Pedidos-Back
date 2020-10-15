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
        public  IActionResult Todos1(string texto, string take = "10", string page = "1")
        {
            int skip = 0;

            if ( string.IsNullOrWhiteSpace(page) || !int.TryParse(page, out _)) page = "1";
            if ( string.IsNullOrWhiteSpace(take) || !int.TryParse(take, out _)) take = "10";

            if(Int32.Parse(page) > 0) skip = (Int32.Parse(page) - 1) * Convert.ToInt32(take);
            
            return Ok(repository.GetAll().Where(x => texto == null || EF.Functions.FreeText(x.RUT, texto) || EF.Functions.FreeText(x.RazonSocial, texto)).Skip(skip).Take(Convert.ToInt32(take)));
            // response = Ok(new {token = token}); x.RUT == texto
        }

    }
}
