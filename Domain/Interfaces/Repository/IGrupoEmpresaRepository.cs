using Domain.Models.Entities;
using Domain.Interfaces.Repositories.RepositoryBase;

namespace Domain.Interfaces.Repositories
{
    public interface IGrupoEmpresaRepository : IRepositoryBase<GrupoEmpresa>
    {
        GrupoEmpresa CheckGrupoEmpresa(int idEmpresa, int idsistema);
    }
}