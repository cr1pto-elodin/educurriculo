using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using API_CSharp.Models;
using System.Reflection;

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

        public bool CheckUser(string cpf)
        {
            try
            {
                using(var db = _db)
                {
                    var retorno = db.Query($"SP_CONSULTAR_USUARIO @CPF = '{cpf}'");
                    if (retorno.Count() == 1) return true;
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int InsertUser(UsuarioModel model)
        {
            using(var db = _db)
            {
                try
                {
                    var linha = db.Execute($@"SP_CADASTRO_ACESSO 
                                                                @USER_NAME = '{model.usuario}',
                                                                @SENHA = '{model.senha}',
                                                                @CPF = '{model.cpf}',
                                                                @NOME = '{model.nome}',
                                                                @DATA_NASCIMENTO = '{model.datanascimento}',
                                                                @SEXO = '{model.sexo}',
                                                                @ESTADO_CIVIL = '{model.ecivil}',
                                                                @ESCOLARIDADE = '{model.escolaridade}',
                                                                @PRETENSAO_SAL = {model.pretensaoSalarial}");
                    return linha;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
