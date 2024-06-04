using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BookingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly string _token;
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(string token, ILogger<WebhookController> logger)
        {
            _token = token;
            _logger = logger;
        }

        [HttpGet("/webhook")]
        public IActionResult HandleSubscription(string hubMode, string hubChallenge, string hubVerifyToken)
        {
            if (hubMode == "subscribe" && hubVerifyToken == _token)
            {
                return Ok(hubChallenge);
            }

            return StatusCode(403); // Forbidden
        }

        [HttpPost("/webhook")]
        public async Task<IActionResult> HandleMessage(dynamic bodyParam)
        {
            if (bodyParam.@object)
            {
                if (bodyParam.entry?.Count > 0 &&
                bodyParam.entry[0].changes?.Count > 0 &&
                bodyParam.entry[0].changes[0].value?.messages?.Count > 0)
                {
                    var phoneNumberId = bodyParam.entry[0].changes[0].value.metadata.phone_number_id;
                    var from = bodyParam.entry[0].changes[0].value.messages[0].from;
                    var msgBody = bodyParam.entry[0].changes[0].value.messages[0].text.body;

                    _logger.LogInformation($"Phone number: {phoneNumberId}");
                    _logger.LogInformation($"From: {from}");
                    _logger.LogInformation($"Body: {msgBody}");

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

                        var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(url, content);
                    }

                    return Ok();
                }

                return NotFound();
            }
            return Ok();
        }
        public class WhatsappBusinessAccountMessage
        {
            public string @object { get; set; } // Use '@' symbol for reserved keywords
            public List<WhatsappBusinessAccountEntry> entry { get; set; }
        }

        public class WhatsappBusinessAccountEntry
        {
            public string id { get; set; }
            public List<WhatsappBusinessAccountChange> changes { get; set; }
        }

        public class WhatsappBusinessAccountChange
        {
            public WhatsappBusinessAccountValue value { get; set; }
            public string field { get; set; }
        }

        public class WhatsappBusinessAccountValue
        {
            public string messaging_product { get; set; }
            public WhatsappBusinessAccountMetadata metadata { get; set; }
            // Add additional properties here as needed to capture specific Webhooks payload data
        }

        public class WhatsappBusinessAccountMetadata
        {
            public string display_phone_number { get; set; }
            public string phone_number_id { get; set; }
        }
    }
}
