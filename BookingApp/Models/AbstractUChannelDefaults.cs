using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BookingApp.Models
{
    public abstract class AbstractUChannelDefaults
    {
        public List<SelectListItem> ChannelsModel { get; set; }
        public int SelectedChannel { get; set; }

    }
}
