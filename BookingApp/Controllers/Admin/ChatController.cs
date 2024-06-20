using BASE.Data.Interfaces.DexTrack;
using BASE.Data.Interfaces;
using BASE.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static BookingApp.Controllers.WhatsappController;
using System.Linq;
using BookingApp.Models;
using System.Globalization;
using System;
using BASE.Model;
using Hangfire.MemoryStorage.Database;
using BASE.CORE.Extensions;

namespace BookingApp.Controllers.Admin
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<UploadController> _logger;
        private readonly IComputerRepository _computerRepository;
        private readonly IUsersDTRepository _usersDTRepository;
        private readonly IUserSessionRepository _userSessionRepository;
        private readonly IUserActionRepository _userActionRepository;
        private readonly IVideosRepository _videosRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWhatsAppChatRepository _whatsAppChatRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatController(IComputerRepository computerRepository, IUsersDTRepository usersDTRepository,
            IUserSessionRepository userSessionRepository, IUserActionRepository userActionRepository,
            IVideosRepository videosRepository,
            IHostEnvironment env, ILogger<UploadController> logger,
             IUnitOfWork unitOfWork, IWhatsAppChatRepository whatsAppChatRepository, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _logger = logger;
            _computerRepository = computerRepository;
            _usersDTRepository = usersDTRepository;
            _userSessionRepository = userSessionRepository;
            _userActionRepository = userActionRepository;
            _videosRepository = videosRepository;
            _unitOfWork = unitOfWork;
            _whatsAppChatRepository = whatsAppChatRepository;
            _httpContextAccessor = httpContextAccessor;


        }
    private readonly JsonResponse response = new JsonResponse();

     [HttpGet]
        public JsonResponse getAll(string Keyword,  int pageIndex, int pageSize)
        {

            try
            {
                var totalRow = 0;
                var query = _whatsAppChatRepository.GetAll()
                    .WhereIf(!string.IsNullOrEmpty(Keyword), x =>  x.FromPhoneNumber.Contains(Keyword)
                || x.ToPhoneNumber.Contains(Keyword)
                || x.Message.Contains(Keyword)
                
                )
                    .ToList()
                    .OrderByDescending(x => x.Time)
                    .AsQueryable()
                    ;
                this.response.Data = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();


                totalRow = query.ToList().Count();
                this.response.Success = true;
                this.response.Pager = totalRow.ToString();
                return this.response;
            }
            catch (Exception ex)
            {
                this.response.Success = false;
                this.response.Message = ex.Message;
                return this.response;
            }

        }
        [HttpGet]
        public WhatsAppChatModel getChat(string chatId)
        {
            var username = _httpContextAccessor.HttpContext.Session.GetString("user");

            var user = _usersDTRepository.GetAll().FirstOrDefault(x => x.Username == username);

            var listChat = _whatsAppChatRepository.GetAll().Where(x => x.ChatId == chatId).ToList();
            var myNum = user.Email;
            var data = new WhatsAppChatModel
            {
                Key = chatId,
                PhoneNumer = listChat.FirstOrDefault().ToPhoneNumber != myNum ? listChat.FirstOrDefault().ToPhoneNumber : listChat.FirstOrDefault().FromPhoneNumber,
                LastMessage = listChat.OrderByDescending(p => p.Time).FirstOrDefault().Message,
                whatsAppChats = listChat.OrderByDescending(x => x.Time).ToList()
            };
            return data;
        }
    }
}
