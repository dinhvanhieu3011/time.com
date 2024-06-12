using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.SecurityMatrix;

namespace BASE.Data.Implements
{
    public class ScreenRepository : BaseRepository<Screen>,IScreenRepository
    {
        public ScreenRepository(AppDbContext context) : base(context)
        {
        }
    }
}
