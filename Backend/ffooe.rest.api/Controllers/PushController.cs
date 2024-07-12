using ffooe.db.context;
using ffooe.rest.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ffooe.rest.api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class PushController : ControllerBase
    {
        private readonly ILogger<PushController> _logger;
        private readonly FFOOEContext _context;
        public PushController(ILogger<PushController> logger, FFOOEContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpPost("callback")]
        public PushSaferResponse PostCallback([FromForm] string json)
        {
            var pushSaferResponse = JsonSerializer.Deserialize<PushSaferResponse>(WebUtility.HtmlDecode(json));
            try
            {
                _logger.LogInformation("----------------------------------------------------------");
                _logger.LogInformation("Callback hit!");
                _logger.LogInformation("----------------------------------------------------------");
                _logger.LogInformation(pushSaferResponse.Action);         // add-device
                _logger.LogInformation(pushSaferResponse.Id);             // device id
                _logger.LogInformation(pushSaferResponse.Name);           // registered name == Personal Guid for PushSafer
                _logger.LogInformation(pushSaferResponse.Guest);          // 1 == Gast

                if (!Action(pushSaferResponse))
                {
                    // ?? why have I done this ?? RemoveDevice(pushSaferResponse.Id);
                    return pushSaferResponse;
                }
                if (pushSaferResponse.Action == "delete-device")
                {
                    if (pushSaferResponse.Id != null) // Delete device from Database DeletePushDevice(pushSaferResponse.Id
                    {
                        _logger.LogInformation($"Device {pushSaferResponse.Id} removedfrom 'PushSaferDevice' Table");
                    }
                    else
                    {
                        _logger.LogError($"Could not remove device {pushSaferResponse.Id} from 'PushSaferDevice' Table");
                    }
                }
                if (pushSaferResponse.Action == "add-device")
                {
                    // Add device to db
                    _logger.LogInformation($"Adding device {pushSaferResponse.Id} to database");
                }
                if (pushSaferResponse.Action == "answer transmitted")
                {
                    // Callback for Answers in Push message

                }
                return pushSaferResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException?.ToString());
                _logger.LogError(ex.StackTrace);
                return pushSaferResponse;
            }

        }
        private bool Action(PushSaferResponse response)
        {
            try
            {
                if (response.Action == "add-device" || response.Action == "delete-device" || response.Action == "answer transmitted")
                {
                    return true;
                }
                _logger.LogError($"Action {response.Action} is not handled");
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.InnerException?.ToString());
                _logger.LogError(ex.StackTrace);
                return false;
            }
        }
    }
}