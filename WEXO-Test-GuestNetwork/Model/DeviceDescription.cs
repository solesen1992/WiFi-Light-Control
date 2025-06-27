using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEXO_Test_GuestNetwork.Model
{
    public class DeviceDescription
    {
        public string description { get; set; }

        public DeviceDescription(string description)
        {
            this.description = description;
        }
    }
}
