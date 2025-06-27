using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Project_BLL.BusinessLogic;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.Facade
{
    public interface IMainFacade
    {
        public void LightsControlWithWiFi(List<DeviceInfo> devices);
        public void StartMonitoringLoop(int checkIntervalSeconds = 10);
    }
}


