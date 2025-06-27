using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Projekt_DAL.DB
{
    public interface IBlackListDB
    {
        List<MACAdressDeviceResponseFromDB> GetBlacklistedMACs();

        List<HostnameDevice> GetBlacklistedHostnames();

        List<DeviceDescriptionResponseFromDB> GetBlackListedDescriptions();

        void CreateDescription(DeviceDescription obj);
        
        void DeleteDescription(string description);
        
        void DeleteHostname(string hostname);
        
        void CreateHostname(HostnameDevice obj);
        
        void CreateMAC(MACAdressDevice obj);
        
        void DeleteMAC(string MAC);


    }
}
