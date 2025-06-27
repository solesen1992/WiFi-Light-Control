using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Project_BLL.BusinessLogic;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.Facade
{
    /*
     * The APIFacade class serves as a simplified interface between the API controllers and the business logic layer.
     * It encapsulates dependencies for settings, blacklist, and login functionality, forwarding calls to the appropriate logic classes.
     * This helps keep the controllers clean and ensures centralized access to core application features.
     */
    public class APIFacade : IAPIFacade
    {
        private readonly IBlackListBusinessLogic _blackListLogic;
        private readonly ISettingsBusinessLogic _settingsLogic;
        private readonly ILoginBusinessLogic _loginBusinessLogic;

        public APIFacade(IBlackListBusinessLogic blackListLogic, ISettingsBusinessLogic settingsLogic, ILoginBusinessLogic loginBusinessLogic)
        {
            _blackListLogic = blackListLogic;
            _settingsLogic = settingsLogic;   
            _loginBusinessLogic = loginBusinessLogic;
        }

        // Settings
        public SettingsConfiguration GetCurrentSettings()
        {
            return _settingsLogic.GetCurrentSettings();
        }

        public void ToggleSystemStatus()
        {
            _settingsLogic.ToggleSystemStatus();
        }

        public void SaveAllSettings(SettingsConfiguration settings)
        {
            _settingsLogic.SaveAll(settings);
        }

        public void SetWeekdayTimeSpan(TimeSpan start, TimeSpan end)
        {
            _settingsLogic.SetWeekdayTimeSpan(start, end);
        }

        public void SetWeekendTimeSpan(TimeSpan start, TimeSpan end)
        {
            _settingsLogic.SetWeekendTimeSpan(start, end);
        }

        public void SetOfflineTimeout(int minutes)
        {
            _settingsLogic.SetOfflineTimeout(minutes);
        }

        // Blacklist
        public List<DeviceDescription> GetBlackListedDescriptions()
        {
            return _blackListLogic.GetBlackListedDescriptions();
        }

        public List<HostnameDevice> GetHostnames()
        {
            return _blackListLogic.GetHostnames();
        }

        public List<MACAdressDevice> GetMACs()
        {
            return _blackListLogic.GetMACs();
        }

        public void AddDescription(DeviceDescription description)
        {
            _blackListLogic.AddDescription(description);
        }

        public void DeleteDescription(string description)
        {
            _blackListLogic.DeleteDescription(description);
        }

        public void AddHostname(HostnameDevice hostname)
        {
            _blackListLogic.AddHostname(hostname);
        }

        public void DeleteHostname(string hostname)
        {
            _blackListLogic.DeleteHostname(hostname);
        }

        public void AddMAC(MACAdressDevice mac)
        {
            _blackListLogic.AddMAC(mac);
        }

        public void DeleteMAC(string mac)
        {
            _blackListLogic.DeleteMAC(mac);
        }

        // Login
        public bool Login(LoginUser user)
        {
            return _loginBusinessLogic.Login(user);
        }
    }
}
