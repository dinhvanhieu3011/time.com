using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace BASE.Data.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync();

        bool Complete();

        IDbContextTransaction BeginTransaction();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CompleteAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool Complete()
        {
            return _context.SaveChanges() > 0;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
    }
}
