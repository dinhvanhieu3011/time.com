using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;

namespace BASE.Data.Implements
{
    public class UsersDTRepository : BaseRepository<Users>,IUsersDTRepository
    {
        public UsersDTRepository(AppDbContext context) : base(context)
        {
        }
    }
}
