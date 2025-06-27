using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.DB;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.Controllers
{
    /**
     * Controller responsible for sorting and filtering devices retrieved from the router.
     * It removes any devices that match blacklisted MAC addresses, hostnames, or descriptions.
     * Uses RouterController to fetch devices and BlackListDB for blacklist validation.
     */
    public class SortingController
    {
        private readonly RouterController _routerController;
        private readonly BlackListDB _blackListDb;

        public SortingController()
        {
            _routerController = new RouterController();
            _blackListDb = new BlackListDB();
        }

        /**
         * Retrieves all currently connected WiFi devices and filters out blacklisted entries.
         * Blacklist filters include: MAC address, hostname (substring match), and description.
         *
         * @return A filtered list of DeviceInfo objects that are considered allowed.
         */
        public List<DeviceInfo> GetDevicesAndSort()
        {
            var allDevices = _routerController.GetWiFiDevices();

            var blacklistedMACs = _blackListDb.GetBlacklistedMACs().Select(x => x.MAC.Trim().ToLower()).ToList();
            var blacklistedHostnames = _blackListDb.GetBlacklistedHostnames().Select(x => x.hostName.Trim().ToLower()).ToList();
            var blacklistedDescriptions = _blackListDb.GetBlackListedDescriptions().Select(x => x.deviceDescription.Trim().ToLower()).ToList();

            var allowedDevices = allDevices
                .Where(device =>
                    !string.IsNullOrWhiteSpace(device.mac) &&
                    !blacklistedMACs.Contains(device.mac.Trim().ToLower()) &&  // Filter blacklisted MACs
                    (string.IsNullOrWhiteSpace(device.hostname) ||
                     !blacklistedHostnames.Any(blacklisted => device.hostname.Trim().ToLower().Contains(blacklisted))) &&  // Filter hostnames based on substring match
                    (string.IsNullOrWhiteSpace(device.descr) ||
                     !blacklistedDescriptions.Contains(device.descr.Trim().ToLower()))  // Filter blacklisted descriptions
                )
                .ToList();

            return allowedDevices;
        }
    }
}
