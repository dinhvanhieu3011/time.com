using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;

namespace BASE.Data.Implements
{
    public class ComputerRepository : BaseRepository<ChannelYoutubes>,IComputerRepository
    {
        public ComputerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
