using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendReminder.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SendReminder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public RemindersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult CreateReminder([FromBody] ReminderRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            if (string.IsNullOrEmpty(request.PhoneNumber) || string.IsNullOrEmpty(request.Message) || request.ReminderTime == default)
            {
                return BadRequest("Missing required fields.");
            }

            var accountSid = _configuration["Twilio:AccountSid"];
            var authToken = _configuration["Twilio:AuthToken"];
            var twilioPhoneNumber = _configuration["Twilio:PhoneNumber"];

            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new Twilio.Types.PhoneNumber(request.PhoneNumber))
            {
                From = new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                Body = request.Message
            };

            messageOptions.SetTimeToSend(request.ReminderTime);

            var message = MessageResource.Create(messageOptions);

            return Ok("Reminder scheduled successfully.");
        }
    }
}
