using Domain.Models.Entities;
using Infra.EntityConfiguration;
using Infra.Interfaces.Repositories;


namespace Infra.Repositories
{
    public class SistemaRepository : RepositoryBase<Sistema>, ISistemaRepository
    {
        private readonly ApplicationDbContext _contex;
        public SistemaRepository(ApplicationDbContext contex) : base(contex)
          => _contex = contex;
    }
}