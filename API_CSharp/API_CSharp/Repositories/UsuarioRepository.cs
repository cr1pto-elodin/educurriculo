using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using API_CSharp.Models;
using System.Reflection;

namespace API_CSharp.Repositories
{
    public class UsuarioRepository
    {
        private readonly SqlConnection _db;
        public UsuarioRepository(IConfiguration _config)
        {
            _db = new SqlConnection(_config.GetConnectionString("DEV"));
            _db.Open();
        }
        public List<UsuarioModel> GetUsuarios()
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

        public bool CheckUser(string cpf)
        {
            try
            {
               var retorno = _db.Query($"SP_CONSULTAR_USUARIO @CPF = '{cpf}'");
               if (retorno.Count() == 1) return true;
               return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertUser(SqlConnection connection,UsuarioModel usuario, SqlTransaction transaction)
        {
            
            connection.Execute($@"SP_CADASTRO_ACESSO",
                new
                {
                    USER_NAME = usuario.acesso.usuario,
                    SENHA = usuario.acesso.senha,
                    CPF = usuario.cpf,
                    NOME = usuario.nome,
                    DATANASCIMENTO = usuario.datanascimento,
                    SEXO = usuario.sexo,
                    ESTADO_CIVIL = usuario.ecivil,
                    ESCOLARIDADE = usuario.escolaridade,
                    PRETENSAO_SAL = usuario.pretensaoSalarial
                }, transaction);
        }

        public void InsertCertificacoes(SqlConnection connection,List<CursoModel> curso, string cpf, SqlTransaction transaction)
        {
            
            foreach(CursoModel certificacao in curso)
            {
                connection.Execute("SP_CADASTRO_CERTIFICACOES",
                    new
                    {
                        CPF = cpf,
                        INSTITUICAO = certificacao.Instituicao,
                        CURSO = certificacao.Curso,
                        INICIO = certificacao.inicio_Curso,
                        FIM = certificacao.Fim_curso
                    }, transaction);
            }
        }

        public void InsertExperiencias(SqlConnection connection, List<ExperienciaModel> experiencias, string cpf, SqlTransaction transaction)
        {
            foreach(ExperienciaModel experiencia in experiencias)
            {
                connection.Execute($@"SP_CADASTRO_EXPERIENCIA",
                    new
                    {
                        CPF = cpf,
                        EMPRESA = experiencia.Empresa,
                        CARGO = experiencia.Cargo,
                        INICIOEX = experiencia.Inicioex,
                        FIMEX = experiencia.Fimex
                    },transaction);
            }
            
        }
    }
}
