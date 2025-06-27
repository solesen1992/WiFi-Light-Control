using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Model for philips hue light
 */

namespace WEXO_Test_GuestNetwork.Model
{
    public class LightState
    {
        public bool on { get; set; }
        public int bri { get; set; }
        public int ct { get; set; }
        public string alert { get; set; }
        public string colormode { get; set; }
        public string mode { get; set; }
        public bool reachable { get; set; }
    }
}
