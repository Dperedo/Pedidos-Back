using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.IdentityModel.Tokens;
using Pedidos_back.Model;
using Pedidos_back.Repository;
using Microsoft.Extensions.Logging;

namespace Pedidos_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsuariosController : ControllerBase
    {
        
        private IUserService userService;

        private IConfiguration config;
        protected ContenerContext context;
        private ILogger<Usuario> logger;

        public UsuariosController(ContenerContext _context,IUserService _userService, IConfiguration _config, ILogger<Usuario> _loggers)
        {
            this.logger = _loggers;
            this.config = _config;
            this.context = _context;
            this.userService = _userService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ObtenerTodos()
        {
            return Ok(userService.GetAll());
        }

        [HttpGet("{name}")]
        [Authorize]
        public IActionResult ObtenerPorNombre(string name)
        {
            return Ok(userService.GetByUserName(name));
        }

        [HttpDelete("{name}")]
        [Authorize]
        public void Eliminar(string name)
        {
            logger.LogInformation("Eliminado");
            userService.Delete(name);
        }

        [HttpPut("{name}")]
        [Authorize]
        public void Actualizar(string name)
        {
            logger.LogInformation("Actualizado");
            var user = userService.GetByUserName(name);
            userService.Update(user);
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UsuarioDto login)
        {
            IActionResult response = Unauthorized();
            
            var token = userService.Authenticate(login);

            if (token != null)
            {
                response = Ok(new {token = token});
                // response = Ok(token);
            }


            return response;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody]UsuarioDto userDto)
        {
            logger.LogInformation("Comienza Registro");
            Usuario user = new Usuario();
            user.Username = userDto.Username;
            try
            {
                userService.Create(user, userDto.Password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
