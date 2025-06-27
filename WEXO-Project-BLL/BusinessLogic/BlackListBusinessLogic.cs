using WEXO_Projekt_DAL.DB;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    /*
     * This class provides business logic for managing blacklisted devices.
     * It interacts with the DAL layer via the IBlackListDB interface to manage blacklisted descriptions, hostnames, and MAC addresses.
     */
    public class BlackListBusinessLogic : IBlackListBusinessLogic
    {
        private readonly IBlackListDB _blackListDB;

        public BlackListBusinessLogic(IBlackListDB blackListDB)
        {
            _blackListDB = blackListDB;
        }

        /*
         * Retrieves all blacklisted descriptions.
         * It calls the data layer to get the descriptions and converts them to DeviceDescription objects.
         * 
         * Returns a list of DeviceDescription objects.
         */
        public List<DeviceDescription> GetBlackListedDescriptions()
        {
            var result = _blackListDB.GetBlackListedDescriptions();
            var descriptions = new List<DeviceDescription>();
            foreach (var res in result)
            {
                descriptions.Add(new DeviceDescription(res.deviceDescription));
            }
            return descriptions;
        }

        /*
         * Retrieves all blacklisted hostnames.
         * Calls the data layer to get the blacklisted hostnames.
         * 
         * Returns a list of HostnameDevice objects.
         */
        public List<HostnameDevice> GetHostnames()
        {
            return _blackListDB.GetBlacklistedHostnames();
        }

        /*
         * Retrieves all blacklisted MAC addresses.
         * It calls the data layer to get the blacklisted MACs and converts them to MACAdressDevice objects.
         * 
         * Returns a list of MACAdressDevice objects.
         */
        public List<MACAdressDevice> GetMACs()
        {
            var result = _blackListDB.GetBlacklistedMACs();
            var macs = new List<MACAdressDevice>();
            foreach (var mac in result)
            {
                macs.Add(new MACAdressDevice(mac.MAC));
            }
            return macs;
        }

        // Adds a description to the blacklist.
        public void AddDescription(DeviceDescription description)
        {
            _blackListDB.CreateDescription(description);
        }

        // Deletes a description from the blacklist.
        public void DeleteDescription(string description)
        {
            _blackListDB.DeleteDescription(description);
        }

        // Adds a hostname to the blacklist.
        public void AddHostname(HostnameDevice hostname)
        {
            _blackListDB.CreateHostname(hostname);
        }

        // Deletes a hostname from the blacklist.
        public void DeleteHostname(string hostname)
        {
            _blackListDB.DeleteHostname(hostname);
        }

        // Adds a MAC address to the blacklist.
        public void AddMAC(MACAdressDevice mac)
        {
            _blackListDB.CreateMAC(mac);
        }

        // Deletes a MAC address from the blacklist.
        public void DeleteMAC(string mac)
        {
            _blackListDB.DeleteMAC(mac);
        }
    }
}
