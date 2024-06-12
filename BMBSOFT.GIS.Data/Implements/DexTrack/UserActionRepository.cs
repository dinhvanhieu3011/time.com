using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;

namespace BASE.Data.Implements
{
    public class UserActionRepository : BaseRepository<UserAction>,IUserActionRepository
    {
        public UserActionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
