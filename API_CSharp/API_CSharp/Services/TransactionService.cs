using API_CSharp.Models;
using API_CSharp.Repositories;
using System.Data.SqlClient;

namespace API_CSharp.Services
{
    public class TransactionService
    {
        private string _connectionString { get; set; }
        private UsuarioRepository _repository;
        public TransactionService(string connectionString, UsuarioRepository repository) { 
            _connectionString = connectionString;
            _repository = repository;
        }

        public int ExecucaoCadastro(UsuarioModel usuario)
        {
            using(var conexao = new SqlConnection(_connectionString))
            {
                conexao.BeginTransaction();
                try
                {
                    SqlTransaction transaction = conexao.BeginTransaction();

                    _repository.InsertUser(conexao, usuario, transaction);
                    _repository.InsertCertificacoes(conexao, usuario.cursos, usuario.cpf, transaction);
                    _repository.InsertExperiencias(conexao,usuario.experiencia,usuario.cpf,transaction);

                    return 1;
                }catch(Exception ex)
                {
                    throw new Exception("Erro de execucao no cadastro: " + ex.Message);
                }

            }
        }
    }
}
