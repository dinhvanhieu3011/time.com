using System;
using System.Collections.Generic;
using System.Linq;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using BASE.Entity.DexTrack;
using BookingApp.Filters.Authorization;
using BookingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookingApp.Controllers
{
    public class ComputerController : Controller
    {
        private readonly ILogger<ComputerController> _logger;
        private readonly IComputerRepository _computerRepository;
        private readonly IUnitOfWork _unitOfWork;


        public ComputerController(ILogger<ComputerController> logger, IComputerRepository computerRepository, IUnitOfWork unitOfWork )
        {
            _logger = logger;
            _computerRepository = computerRepository;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Index()
        {
            var list = _computerRepository.GetAll().ToList();
            List<ChannelYoutubeModel> lst = new List<ChannelYoutubeModel>();

            if (list.Count > 0) {
                foreach (var item in list)
                {

                    lst.Add(new ChannelYoutubeModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Link = item.Token,
                        Language = "",
                        UserId = item.EmployeeName,
                        Category ="",
                        Status = item.Status == "" ? "Mất kết nối" : "Đang online"
                    });

                }
            }
            return View(lst);

        }
        #region create
        public IActionResult Create()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(string Name, string Link, string UserId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Computer");
            }
            Link = Guid.NewGuid().ToString();
            switch (Insert(Name, Link,UserId))
            {
                case true:
                    _logger.LogInformation("Tạo Máy " + Name + " thành công!");
                    return RedirectToAction("Index", "Computer", new { msg = "added" });
                case false:
                    return RedirectToAction("Index", "Computer", new { error = "wrongName" });
                case null:
                    return RedirectToAction("Index", "Computer", new { error = "error" });
            }
        }
        private bool? Insert(string ComputerName, string Token, string EmployeeName)
        {
            try
            {
                ChannelYoutubes channel = new ChannelYoutubes();
                channel.Name = ComputerName;
                channel.Token = Token;
                channel.Token = EmployeeName;
                _computerRepository.Insert(channel);
                _unitOfWork.Complete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region update
        public IActionResult Update(int id)
        {
            var book = _computerRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

            if (book != null)
            {

                _logger.LogInformation("Chỉnh sửa Máy " + book.Id + " thành công!");
                return View(book);
            }
            else
            {
                return RedirectToAction("Index", "Computer", new { error = "wrongData" });
            }
        }
        public IActionResult Live(int id)
        {
            var book = _computerRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();

            if (book != null)
            {
                ViewBag.Ip = @"/live/" + book.Token+".png";
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Computer", new { error = "wrongData" });
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Update(int id, string Name, string Link, string UserId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Library");
                }
                if (id > 0 &&  !string.IsNullOrEmpty(Name)   
                    )
                {

                    UpdateData(id, Name, Link, UserId);
                    return RedirectToAction("Update", "Computer", new { id, msg = "updated" });
                }
                else
                {
                    return RedirectToAction("Index", "Computer", new { id, error = "error" });
                }
            }
            catch
            {
                return RedirectToAction("Index", "Computer", new { id, error = "error" });
            }
        }

        //Can be rented could be a solution to the point of a stolen book
        private void UpdateData(int id, string Name, string Link, string UserId)
        {
            var data = _computerRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();
            data.Name = Name;
            data.Token = Link;
            data.EmployeeName = UserId;
            _computerRepository.Update(data);
            _unitOfWork.Complete();
        }
        #endregion
        public IActionResult Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Computer");
                }
                var com = _computerRepository.GetAll().Where(x=>x.Id == id).FirstOrDefault();
                com.Status = "0";
                _computerRepository.Update(com);
                _unitOfWork.Complete();
                _logger.LogInformation("Xoá Máy " + id + " thành công!");

                return RedirectToAction("Index", "Computer", new { msg = "Deleted" });

            }
            catch
            {
                return RedirectToAction("Index", "Computer", new { error = "error" });
            }
        }
    }
}
