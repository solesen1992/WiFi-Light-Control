using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;
using WEXO_Projekt_DAL.DB;
using System.Globalization;

namespace WEXO_Project_BLL.BusinessLogic
{
 /*
 * Business logic for filtering connected devices based on a blacklist.
 * This class retrieves all connected devices from the router and removes those that match
 * blacklist criteria such as MAC addresses, hostnames, or descriptions.
 */
    public class SortingBusinessLogic : ISortingBusinessLogic
    {
        private readonly IRouterBusinessLogic _routerLogic;
        private readonly IBlackListDB _blackListDb;

        public SortingBusinessLogic(IRouterBusinessLogic routerBusinessLogic, IBlackListDB blackListDB)
        {
            _routerLogic = routerBusinessLogic;
            _blackListDb = blackListDB;
        }

        /*
         * Retrieves all connected devices from the router and filters out those
         * that match any blacklist criteria (MAC address, hostname, or description).
         *
         * Returns:
         *   A list of devices that are not blacklisted.
         */
        public List<DeviceInfo> GetDevicesAndSort()
        {
            var allDevices = _routerLogic.GetAll();

            // Load and normalize blacklist values for comparison
            var blacklistedMACs = _blackListDb.GetBlacklistedMACs().Select(x => x.MAC.Trim().ToLower()).ToList();
            var blacklistedHostnames = _blackListDb.GetBlacklistedHostnames().Select(x => x.hostName.Trim().ToLower()).ToList();
            var blacklistedDescriptions = _blackListDb.GetBlackListedDescriptions().Select(x => x.deviceDescription.Trim().ToLower()).ToList();

            // Filter out blacklisted devices
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

            // Return filtered list
            return allowedDevices;
        }
    }
}
