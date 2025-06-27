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
    public class Light
    {
        public string Id { get; set; }

        public LightState State { get; set; }
    }
}
