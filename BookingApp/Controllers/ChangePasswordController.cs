using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BookingApp.DB.Classes;
using BookingApp.Filters.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace BookingApp.Controllers
{
    [Authorize(Roles.CUSTOMER, Roles.ADMIN)]
    public class ChangePasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly ILogger<ChangePasswordController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePasswordController(IHttpContextAccessor httpContextAccessor,
            ILogger<ChangePasswordController> logger, IUsersDTRepository usersDTRepository, IUnitOfWork unitOfWork )
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _usersDTRepository = usersDTRepository;
            _unitOfWork = unitOfWork;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string oldPassword, string newPassword, string rPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "ChangePassword");
                }

                if (newPassword != rPassword)
                {
                    return RedirectToAction("Index", "ChangePassword", new { error = "wrongPasswords" });
                }

                var username = _httpContextAccessor.HttpContext.Session.GetString("user");

                var user = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == username);

                if (Encryption.Encrypt(oldPassword).Equals(user.Password))
                {
                    user.Password = Encryption.Encrypt(newPassword);
                    _usersDTRepository.Update(user);
                    _unitOfWork.Complete();
                    _logger.LogInformation("Tài khoản " + username + " đổi mật khẩu thành công!");

                    return RedirectToAction("Index", "ChangePassword", new { msg = "updated" });

                }
                else
                {
                    return RedirectToAction("Index", "ChangePassword", new { error = "wrongOld" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "ChangePassword", new { error = "error" });
            }
        }
    }
}