using BASE.Data.Interfaces;
using BASE.Data.Repository;

namespace BASE.Data.Implements
{
    public class ActionRepository : BaseRepository<Entity.SecurityMatrix.Action>,IActionRepository
    {
        public ActionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
