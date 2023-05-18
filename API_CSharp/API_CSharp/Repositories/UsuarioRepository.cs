using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using API_CSharp.Models;

namespace API_CSharp.Repositories
{
    public class UsuarioRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _db;
        public UsuarioRepository(IConfiguration _config) {
            _configuration = _config;
            _db = new SqlConnection(_config.GetConnectionString("DEV"));
        }
        public List<UsuarioModel> GetUsuarios()
        {
            using(_db)
            {
                List<UsuarioModel> usuario = _db.Query<UsuarioModel>(
                    @"SELECT 
                        ID,
                        [USER_NAME],
                        CPF,
                        ATIVO,
                        NOME,
                        DATANASCIMENTO,
                        SEXO,
                        ESTADO_CIVIL, 
                        ESCOLARIDADE
                    FROM ACESSO INNER JOIN CADASTRO ON ACESSO.ID = CADASTRO.ID_ACESSO").ToList();
                return usuario;
            }
        }
    }
}
