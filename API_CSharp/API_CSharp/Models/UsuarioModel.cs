using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace API_CSharp.Models
{
    public class UsuarioModel
    {
        public int? id { get; set; }
        public string nome { get; set; }
        public string senha { get; set; }
        public string csenha { get; set; }
        public string cpf { get; set; }
        public string usuario { get; set; }
        public List<ExperienciaModel> Experiencia { get; set; }
        public List<CursoModel> Cursos { get; set; }
        public string sexo { get; set; }
        public string ecivil { get; set; }
        public DateTime datanascimento { get; set; }
        public decimal pretensaoSalarial { get; set; }
        public bool ativo { get; set; }

    }
}
