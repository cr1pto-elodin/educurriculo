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
    public class Usuario : ControllerBase
    {
        private readonly UsuarioRepository _repository;
        public Usuario(UsuarioRepository usuarioRepository) {
            _repository= usuarioRepository;
        }
        [HttpPost]
        public ActionResult<string> setUsuario([FromBody]API_CSharp.Models.Usuario usuario)
        {
            try
            {
                if (usuario == null) return BadRequest(new { message = "Argumento vazio" });

                //var usuario = JsonConvert.DeserializeObject<Usuario>(request);

                //string senha = CriptografaService.EncryptMD5(usuario.senha);

                //return Ok(JsonConvert.SerializeObject(new { senhaCripto =  senha}));
                return Ok(JsonConvert.SerializeObject(usuario));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(Name = "GetUsuario")]
        public ActionResult<List<API_CSharp.Models.Usuario>> getUsuario()
        {
            return _repository.GetUsuarios();
        }
    }
}
