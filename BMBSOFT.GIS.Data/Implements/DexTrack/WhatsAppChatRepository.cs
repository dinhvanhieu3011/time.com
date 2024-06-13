using BASE.Data.Interfaces;
using BASE.Data.Interfaces.DexTrack;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASE.Data.Implements.DexTrack
{
    public class WhatsAppChatRepository : BaseRepository<WhatsAppChat>, IWhatsAppChatRepository
    {
        public WhatsAppChatRepository(AppDbContext context) : base(context)
        {
        }
    }
}
