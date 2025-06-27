using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.Facade
{
    public interface IAPIFacade
    {
        public SettingsConfiguration GetCurrentSettings();
        public void ToggleSystemStatus();
        public void SaveAllSettings(SettingsConfiguration settings);
        public void SetWeekdayTimeSpan(TimeSpan start, TimeSpan end);
        public void SetWeekendTimeSpan(TimeSpan start, TimeSpan end);
        public void SetOfflineTimeout(int minutes);
        public List<DeviceDescription> GetBlackListedDescriptions();
        public List<HostnameDevice> GetHostnames();
        public List<MACAdressDevice> GetMACs();
        public void AddDescription(DeviceDescription description);
        public void DeleteDescription(string description);
        public void AddHostname(HostnameDevice hostname);
        public void DeleteHostname(string hostname);
        public void AddMAC(MACAdressDevice mac);
        public void DeleteMAC(string mac);
        public bool Login(LoginUser user);

    }
}
