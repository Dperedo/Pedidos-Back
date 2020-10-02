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
    public class ClientesController : BaseController<Cliente>
    {
        private IRepository<Cliente> repository;

        public ClientesController(IRepository<Cliente> _repository) : base(_repository)
        {
            repository = _repository;
        }

        [HttpGet]
        public override IActionResult Todos()
        {
            return Ok(repository.GetAll());
            // response = Ok(new {token = token});
        }

    }
}
