using Domain.DTO;

namespace Domain.Models.Entities
{
    public class GrupoEmpresa
    {
     
        public int GrupoEmpresaId { get; set; }
        public int MatrizIdExterno { get; set; }
        public int EmpresaId { get; set; }
        public int SistemaId { get; set; }
    }
}