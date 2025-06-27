using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Projekt_DAL.APIConsumers
{
    public interface IRouterAPIConsumer
    {
        // Metode til at hente alle devices
        IList<DeviceInfo> GetAll();
    }
}
