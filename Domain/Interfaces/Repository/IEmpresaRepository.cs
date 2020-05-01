using Domain.Models.Entities;
using Domain.Interfaces.Repositories.RepositoryBase;
using System.Collections.Generic;

namespace Domain.Interfaces.Repositories
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