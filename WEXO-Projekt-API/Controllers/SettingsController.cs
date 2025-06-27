using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEXO_Projekt_DAL.Model;
using WEXO_Project_BLL.Facade;

namespace WEXO_Projekt_API.Controllers
{
 /**
 * SettingsController
 * 
 * Exposes endpoints for managing system settings related to lighting automation.
 * This includes toggling the system on/off, setting active hours for weekdays and weekends,
 * configuring the offline timeout, and saving the complete settings configuration.
 * The controller delegates all logic to the APIFacade to maintain separation of concerns.
 */
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly IAPIFacade _facade;

        public SettingsController(IAPIFacade apifacade)
        {
            _facade = apifacade;
        }

        [HttpGet]
        public ActionResult<SettingsConfiguration> GetCurrentSettings()
        {
            return Ok(_facade.GetCurrentSettings());
        }

        [HttpPut("toggleSystem")]
        public IActionResult ToggleSystemStatus()
        {
            _facade.ToggleSystemStatus();
            return Ok();
        }

        [HttpPut("saveAll")]
        public IActionResult SaveAll([FromBody] SettingsConfiguration updatedSettings)
        {
            _facade.SaveAllSettings(updatedSettings);
            return Ok(updatedSettings);
        }

        [HttpPut("setWeekdayTime")]
        public IActionResult SetWeekdayTimeSpan([FromQuery] TimeSpan start, [FromQuery] TimeSpan end)
        {
            _facade.SetWeekdayTimeSpan(start, end);
            return Ok(new { start, end });
        }

        [HttpPut("setWeekendTime")]
        public IActionResult SetWeekendTimeSpan([FromQuery] TimeSpan start, [FromQuery] TimeSpan end)
        {
            _facade.SetWeekendTimeSpan(start, end);
            return Ok(new { start, end });
        }

        [HttpPut("setTimeout")]
        public IActionResult SetOfflineTimeout([FromQuery] int minutes)
        {
            _facade.SetOfflineTimeout(minutes);
            return Ok(minutes);
        }
    }
}

