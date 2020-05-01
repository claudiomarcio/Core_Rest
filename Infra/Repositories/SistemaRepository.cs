using Domain.Interfaces.Repositories;
using Domain.Models.Entities;
using Domain.EntityConfiguration;


namespace Domain.Repositories
{
    public class SistemaRepository : RepositoryBase<Sistema>, ISistemaRepository
    {
        private readonly ApplicationDbContext _contex;
        public SistemaRepository(ApplicationDbContext contex) : base(contex)
          => _contex = contex;
    }
}