using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Model for devices such as phone, pc etc. that is on the network.
 */

namespace WEXO_Test_GuestNetwork.Model
{
    public class DeviceInfo
    {
        public int id { get; set; }
        public string ip { get; set; }
        public string mac { get; set; }
        public string hostname { get; set; }
        public string starts { get; set; }
        public string ends { get; set; }
        public string active_status { get; set; }
        public string online_status { get; set; }
        public string descr { get; set; }
    }
}
