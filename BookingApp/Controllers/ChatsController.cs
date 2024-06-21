using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    public class ChatsController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
