using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;

namespace BASE.Data.Implements
{
    public class UserSessionRepository : BaseRepository<UserSession>,IUserSessionRepository
    {
        public UserSessionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
