using Domain.Models.Entities;
using Infra.Interfaces.Repositories.RepositoryBase;

namespace Infra.Interfaces.Repositories
{
    public interface IGrupoEmpresaRepository : IRepositoryBase<GrupoEmpresa>
    {
        GrupoEmpresa CheckGrupoEmpresa(int idEmpresa, int idsistema);
    }
}