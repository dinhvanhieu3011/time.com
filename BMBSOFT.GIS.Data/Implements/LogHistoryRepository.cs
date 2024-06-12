using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.LogHistory;

namespace BASE.Data.Implements
{
    public class LogHistoryRepository : BaseRepository<LogHistoryEntity>, ILogHistoryRepository
    {
        public LogHistoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
