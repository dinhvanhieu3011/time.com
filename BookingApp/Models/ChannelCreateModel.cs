using BookingApp.DB.Classes.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Models
{
    public class ChannelCreateModel : AbstractUChannelDefaults
    {
        //public ChannelCreateModel()
        //{

        //    using var db = new AppDbContext();

        //    ChannelsModel = db.Books.ToList().Select
        //        (
        //        x => new SelectListItem { Value = x.BookId.ToString(), Text = x.Name }
        //        ).ToList();

        //    SelectedChannel = 1;
        //}
    }
}
