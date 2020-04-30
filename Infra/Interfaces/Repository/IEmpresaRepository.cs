using Domain.Models.Entities;
using Infra.Interfaces.Repositories.RepositoryBase;
using System.Collections.Generic;

namespace Infra.Interfaces.Repositories
{
    public interface IEmpresaRepository : IRepositoryBase<Empresa>
    {
        Empresa CheckEmpresa(string nome);
        List<GrupoEmpresa> GetEmpresasDoGrupo(int ID);
        List<GrupoEmpresa> GetEmpresasDoGrupo();
        Empresa BuscarEmpresa(int empresaIdExterno, int sistemaId);

        bool RemoverEmpresa(int id);
    }
}