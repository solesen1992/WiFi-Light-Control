using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_Sorting_Blacklist.Model;
using WEXO_Test_Sorting_Blacklist.DB;

namespace WEXO_Test_Sorting_Blacklist.Controllers
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
            var allDeviceDTOs = _routerController.GetWiFiDevices();

            var blacklistedMACs = _blackListDb.GetBlacklistedMACs().Select(x => x.MAC.Trim().ToLower()).ToList();
            var blacklistedHostnames = _blackListDb.GetBlacklistedHostnames().Select(x => x.hostName.Trim().ToLower()).ToList();
            var blacklistedDescriptions = _blackListDb.GetBlackListedDescriptions().Select(x => x.deviceDescription.Trim().ToLower()).ToList();

            Console.WriteLine("Blacklisted MACs: ");
            foreach (var mac in blacklistedMACs)
            {
                Console.WriteLine(mac);
            }

            Console.WriteLine(" ");

            Console.WriteLine("Blacklisted Hostnames: ");
            foreach (var hostname in blacklistedHostnames)
            {
                Console.WriteLine(hostname);
            }

            Console.WriteLine(" ");

            Console.WriteLine("Blacklisted Descriptions: ");
            foreach (var descr in blacklistedDescriptions)
            {
                Console.WriteLine(descr);
            }

            Console.WriteLine(" ");

            var allowedDevices = allDeviceDTOs
                .Where(dto =>
                    !string.IsNullOrWhiteSpace(dto.mac) &&
                    !blacklistedMACs.Contains(dto.mac.Trim().ToLower()) &&  // Filter blacklisted MACs
                    (string.IsNullOrWhiteSpace(dto.hostname) ||
                     !blacklistedHostnames.Any(blacklisted => dto.hostname.Trim().ToLower().Contains(blacklisted))) &&  // Filter hostnames based on substring match
                    (string.IsNullOrWhiteSpace(dto.descr) ||
                     !blacklistedDescriptions.Contains(dto.descr.Trim().ToLower()))  // Filter blacklisted descriptions
                )
                .ToList();

            return allowedDevices;
        }
    }
}
