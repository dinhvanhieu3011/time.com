using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.IdentityAccess;

namespace BASE.Data.Implements
{
    public class UserRepository : BaseRepository<User>,IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
