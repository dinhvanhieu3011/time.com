using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using BASE.Data.Interfaces.DexTrack;
using BASE.Data.Repository;

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWhatsAppChatRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public WebhookController(ILogger<WebhookController> logger, IConfiguration configuration, IWhatsAppChatRepository repository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("/webhook")]
        public IActionResult HandleSubscription([FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string token)
        {
            if (mode == "subscribe" && token == "apple")
            {
                return Ok(challenge);
            }

            return StatusCode(403); // Forbidden
        }

        [HttpPost("/webhook")]
        public async Task<IActionResult> HandleMessage(Root bodyParam)
        {


            try
            {
                string jsonString1 = JsonConvert.SerializeObject(bodyParam);

                _logger.LogError(jsonString1);

                if (!string.IsNullOrEmpty((string)bodyParam.@object))
                {
                    //string jsonString = JsonConvert.SerializeObject(bodyParam);

                    //_logger.LogError(jsonString);

                    //var data = (Root)bodyParam;
                    Root data = bodyParam;
                    string _token = "EAAL9UWkg1NcBO0zpx77dv1d3VOpdDA93yBNc29UYGDJZCgkqCTNpfi8HnutZCzZBc4iY911j4zquokPZA6vE0HceZBs92d77qnqKhgTgiKj4z7EdZAeS0XW8ZC8wRtQvC6pG34MeMsStxsycgbfsK9itRfRbSKNudh2pwEKIooIzHyCa2KNzyKRypLFGvRvQSsxZB6gzelAZBH1fMignFaZCIZD";
                    if (data.entry?.Count > 0 &&
                    data.entry[0].changes?.Count > 0 &&
                    data.entry[0].changes[0].value != null)
                    {
                        _repository.Insert(new BASE.Entity.DexTrack.WhatsAppChat
                        {
                            Id = Guid.NewGuid().ToString(),
                            ChatId = data.entry[0].id,
                            Timestamp = long.Parse(data.entry[0].changes[0].value.messages[0].timestamp),
                            FromId = "",
                            FromPhoneNumber = data.entry[0].changes[0].value.messages[0].from,
                            ToId = data.entry[0].changes[0].value.metadata.phone_number_id,
                            ToPhoneNumber = data.entry[0].changes[0].value.metadata.display_phone_number,
                            Message = data.entry[0].changes[0].value.messages[0].text.body,
                            Time = new DateTime(long.Parse(data.entry[0].changes[0].value.messages[0].timestamp))
                        });
                        _unitOfWork.Complete();

                        var client = new HttpClient();
                        string url1 = $"http://jera.drayddns.com/api/Upload/CreateMessage?ChatId={data.entry[0].id}" +
                            $"&Timestamp={data.entry[0].changes[0].value.messages[0].timestamp}" +
                            $"&FromPhoneNumber={data.entry[0].changes[0].value.messages[0].from}" +
                            $"&FromId=&ToPhoneNumber={data.entry[0].changes[0].value.metadata.display_phone_number}" +
                            $"&ToId={data.entry[0].changes[0].value.metadata.phone_number_id}" +
                            $"&Message={data.entry[0].changes[0].value.messages[0].text.body}";
                        var request = new HttpRequestMessage(HttpMethod.Post, url1);
                        request.Headers.Add("accept", "text/plain");
                        var response = await client.SendAsync(request);
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine(await response.Content.ReadAsStringAsync());


                        var phoneNumberId = data.entry[0].changes[0].value.metadata.phone_number_id;
                        var from = data.entry[0].changes[0].value.messages[0].from;
                        var msgBody = data.entry[0].changes[0].value.messages[0].text.body;
                        using (var httpClient = new HttpClient())
                        {
                            var url = $"https://graph.facebook.com/v13.0/{phoneNumberId}/messages?access_token={_token}";
                            var message = new
                            {
                                messaging_product = "whatsapp",
                                to = from,
                                text = new
                                {
                                    body = $"{msgBody}"
                                }
                            };

                            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
                            await httpClient.PostAsync(url, content);
                        }

                        return Ok();
                    }

                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return Ok();
        }
        public class Change
        {
            public Value value { get; set; }
            public string field { get; set; }
        }

        public class Contact
        {
            public Profile profile { get; set; }
            public string wa_id { get; set; }
        }

        public class Entry
        {
            public string id { get; set; }
            public List<Change> changes { get; set; }
        }

        public class Message
        {
            public string from { get; set; }
            public string id { get; set; }
            public string timestamp { get; set; }
            public Text text { get; set; }
            public string type { get; set; }
        }

        public class Metadata
        {
            public string display_phone_number { get; set; }
            public string phone_number_id { get; set; }
        }

        public class Profile
        {
            public string name { get; set; }
        }

        public class Root
        {
            public string @object { get; set; }
            public List<Entry> entry { get; set; }
        }

        public class Text
        {
            public string body { get; set; }
        }

        public class Value
        {
            public string messaging_product { get; set; }
            public Metadata metadata { get; set; }
            public List<Contact> contacts { get; set; }
            public List<Message> messages { get; set; }
        }






    }
}
