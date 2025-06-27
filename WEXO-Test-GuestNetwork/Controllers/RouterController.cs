using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.APIConsumers;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.Controllers
{
    /**
     * Controller responsible for accessing WiFi device information via the router's API.
     * Provides methods to retrieve a list of currently connected devices.
     */
    public class RouterController
    {
        private readonly RouterAPIConsumer _routerApiConsumer;

        public RouterController()
        {
            _routerApiConsumer = new RouterAPIConsumer();
        }

        /**
         * Retrieves all devices currently connected to the WiFi network.
         * Wraps the API call and returns a list of DeviceInfo objects.
         * Throws an exception if the API call fails.
         *
         * @return A list of connected devices retrieved from the router.
         * @throws Exception if the router API fails to return device data.
         */
        public IList<DeviceInfo> GetWiFiDevices()
        {
            try
            {
                return new List<DeviceInfo>(_routerApiConsumer.GetWiFiDevices());
            }
            catch (Exception ex)
            {
                throw new Exception("Fejl ved hentning af enheder fra router", ex);
            }
        }
    }
}
