using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;
using WEXO_Projekt_DAL.DB;

namespace WEXO_Project_BLL.BusinessLogic
{
    public interface IBlackListBusinessLogic
    {
        List<DeviceDescription> GetBlackListedDescriptions();
        List<HostnameDevice> GetHostnames();
        List<MACAdressDevice> GetMACs();
        void AddDescription(DeviceDescription description);
        void DeleteDescription(string description);
        void AddHostname(HostnameDevice hostname);
        void DeleteHostname(string hostname);
        void AddMAC(MACAdressDevice mac);
        void DeleteMAC(string mac);

    }
}
