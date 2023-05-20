using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft;
using Newtonsoft.Json;
using API_CSharp.Models;
using API_CSharp.Services;
using API_CSharp.Repositories;
using System.Text.Json.Nodes;

namespace API_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRepository _repository;
        public UsuarioController(UsuarioRepository usuarioRepository) {
            _repository= usuarioRepository;
        }
        [HttpPost]
        public ActionResult<string> setUsuario([FromBody] UsuarioModel usuario)
        {
            try
            {
                if (usuario == null) return BadRequest(new { message = "Argumento vazio" });

                usuario.acesso.senha = CriptografaService.EncryptMD5(usuario.acesso.senha);

                if (_repository.CheckUser(usuario.cpf)) return Conflict(new { message = "Usuário já cadastrado! Favor logar!" });

                _repository.InsertUser(usuario);

                return Ok(JsonConvert.SerializeObject(usuario));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetUsuario")]
        public ActionResult<List<API_CSharp.Models.UsuarioModel>> getUsuario()
        {
            return _repository.GetUsuarios();
        }
    }
}
