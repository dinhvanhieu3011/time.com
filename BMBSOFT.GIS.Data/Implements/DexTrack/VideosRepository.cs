using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;

namespace BASE.Data.Implements
{
    public class VideosRepository : BaseRepository<Videos>,IVideosRepository
    {
        public VideosRepository(AppDbContext context) : base(context)
        {
        }
    }
}
