using Microsoft.AspNetCore.Mvc;
using WEXO_Projekt_DAL.Model;
using WEXO_Project_BLL.BusinessLogic;
using WEXO_Project_BLL.Facade;


namespace WEXO_Projekt_API.Controllers
{
 /**
 * BlacklistController
 * 
 * Provides API endpoints for managing a blacklist of devices based on different criteria:
 * descriptions, hostnames, and MAC addresses.
 * Delegates the core logic of adding, removing, and retrieving blacklisted devices to the APIFacade.
 * Each action corresponds to a specific HTTP method (GET, POST, DELETE) for interacting with the blacklist.
 */
    [ApiController]
    [Route("[controller]")]
    public class BlacklistController : ControllerBase
    {
        private readonly IAPIFacade _facade;

        public BlacklistController(IAPIFacade apifacade)
        {
            _facade = apifacade;
        }

        [HttpGet]
        [Route("/Blacklist/description")]
        public ActionResult<List<DeviceDescription>> Get() 
        {
            var result = _facade.GetBlackListedDescriptions();
            return Ok(result);
        }

        [HttpGet]
        [Route("/Blacklist/hostname")]
        public ActionResult<List<HostnameDevice>> GetHostNames()
        {
            var result = _facade.GetHostnames();
            return Ok(result);
        }

        [HttpGet]
        [Route("/Blacklist/macAdress")]
        public ActionResult<List<MACAdressDevice>> GetMAC()
        {
            var result = _facade.GetMACs();
            return Ok(result);
        }

        [HttpPost]
        [Route("/Blacklist/description")]
        public ActionResult<DeviceDescription> PostDescription([FromBody] DeviceDescription obj)
        {
            _facade.AddDescription(obj);
            return Ok(obj);
        }

        [HttpDelete]
        [Route("/Blacklist/description/{description}")]
        public ActionResult<DeviceDescription> DeleteDescription(string description)
        {
            _facade.DeleteDescription(description);
            return Ok();
        }

        [HttpDelete]
        [Route("/Blacklist/hostname/{hostname}")]
        public ActionResult<HostnameDevice> DeleteHostname(string hostname)
        {
            _facade.DeleteHostname(hostname);
            return Ok();
        }

        [HttpPost]
        [Route("/Blacklist/hostname")]
        public ActionResult<HostnameDevice> PostHostname([FromBody] HostnameDevice obj)
        {
            _facade.AddHostname(obj);
            return Ok(obj);
        }

        [HttpPost]
        [Route("/Blacklist/macAdress")]
        public ActionResult<MACAdressDevice> PostMAC([FromBody] MACAdressDevice obj)
        {
            _facade.AddMAC(obj);
            return Ok(obj);
        }

        [HttpDelete]
        [Route("/Blacklist/macAdress/{MAC}")]
        public ActionResult<HostnameDevice> DeleteMAC(string MAC)
        {
            _facade.DeleteMAC(MAC);
            return Ok();
        }

    }
}
