using Domain.DTO;
using Domain.Interfaces.Repositories;
using Domain.Models.Entities;
using Domain.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Domain.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        private readonly ApplicationDbContext _contex;
        public EmpresaRepository(ApplicationDbContext contex) : base(contex)
          => _contex = contex;

        //public async Task<Empresa> BuscarEmpresa(int id, int sistemaId)
        //{
        //    return await _contex.Empresa.Where(x => x.IdExterno == id && x.SistemaId == sistemaId).Select(x => x).FirstOrDefaultAsync();
        //}

        public Empresa BuscarEmpresa(int empresaIdExterno, int sistemaId)
                  => (from p in _contex.Empresa
                      select new { p }).Where(x => x.p.IdExterno == empresaIdExterno &&  x.p.SistemaId == sistemaId).Select(x => x.p).FirstOrDefault();
 
        public Empresa CheckEmpresa(string nome)
        {
            return _contex.Empresa.Where(x => x.Nome.Equals(nome)).FirstOrDefault();
        }

        public bool RemoverEmpresa(int id)
        {
            Empresa emp = GetById(id);
            if (emp == null)
                return false;

            emp.Ativo = false;

            try
            {
                this.Update(emp);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public List<GrupoEmpresa> GetEmpresasDoGrupo(int ID)
        {
            return _contex.GrupoEmpresa.Where(x => x.EmpresaId == ID).ToList();
        }

        public List<GrupoEmpresa> GetEmpresasDoGrupo()
        {
            return _contex.GrupoEmpresa.ToList();
        }
    }
}