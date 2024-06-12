using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;
        private readonly IConfiguration _configuration;

        public WebhookController(ILogger<WebhookController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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
        public async Task<IActionResult> HandleMessage(dynamic bodyParam)
        {

            _logger.LogError("run");
            string jsonString = JsonConvert.SerializeObject(bodyParam);
            _logger.LogError(jsonString);

            _logger.LogError((string)bodyParam.@object);
            try
            {
                if (!string.IsNullOrEmpty((string)bodyParam.@object))
                {
                    //var data = (Root)bodyParam;
                    // Convert dynamic object to JSON string

                    // Deserialize JSON string to Root object
                    Root data = JsonConvert.DeserializeObject<Root>(jsonString);
                    string _token = _configuration?.GetSection("WhatsApp")["WhatsApp"];
                    if (data.entry?.Count > 0 &&
                    data.entry[0].changes?.Count > 0 &&
                    data.entry[0].changes[0].value != null)
                    {
                        var phoneNumberId = data.entry[0].changes[0].value.metadata.phone_number_id;
                        var from = data.entry[0].changes[0].value.messages[0].from;
                        var msgBody = data.entry[0].changes[0].value.messages[0].text.body;

                        _logger.LogError($"Phone number: {phoneNumberId}");
                        _logger.LogError($"From: {from}");
                        _logger.LogError($"Body: {msgBody}");

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

            }
            return Ok();
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
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
