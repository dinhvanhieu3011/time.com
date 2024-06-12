using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.IdentityAccess;

namespace BASE.Data.Implements
{
    public class RoleRepository : BaseRepository<Role>,IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
