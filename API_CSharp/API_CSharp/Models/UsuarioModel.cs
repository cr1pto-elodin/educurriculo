using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API_CSharp.Models
{
    public class UsuarioModel
    {
        public int? id { get; set; }
        public AcessoModel acesso { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public List<ExperienciaModel> experiencia { get; set; }
        public List<CursoModel> cursos { get; set; }
        public string sexo { get; set; }
        public string ecivil { get; set; }
        public string escolaridade   { get; set; }
        public DateTime datanascimento { get; set; }
        public decimal pretensaoSalarial { get; set; }
        public bool ativo { get; set; }

    }
}
