using Domain.DTO;

namespace Domain.Models.Entities
{
    public class Empresa
    {
        
        public Empresa(){}
     
        public Empresa(EmpresaMotoristaDTO empresa)
        {
            IdExterno = empresa.id;
            Nome = empresa.apelido.Trim() ?? empresa.nome.Trim();
            SistemaId = empresa.idSistema;
        }

     
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public int IdExterno { get; set; }   
        
        public bool Ativo { get; set; }
        //Relacionamento
        public int SistemaId { get; set; }
        public virtual Sistema Sistema { get; set; }
    }
}