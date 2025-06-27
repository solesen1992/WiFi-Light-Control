using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;
using WEXO_Projekt_DAL.APIConsumers;

/*
 * This class provides business logic for interacting with the router's API through 
 * the IRouterAPIConsumer.
 */

namespace WEXO_Project_BLL.BusinessLogic
{
    public class RouterBusinessLogic : IRouterBusinessLogic
    {
        private readonly IRouterAPIConsumer _routerApiConsumer;

        public RouterBusinessLogic(IRouterAPIConsumer routerAPIConsumer)
        {
            _routerApiConsumer = routerAPIConsumer;
        }

        /*
         * Retrieves all connected devices from the router via the IRouterAPIConsumer.
         * The method calls GetAll() on IRouterAPIConsumer to retrieve the list of connected devices.
         * 
         * Returns a list of DeviceInfo objects, each containing details about a connected device.
         * If there is an issue while retrieving the data from the router, an exception is thrown.
         */
        public IList<DeviceInfo> GetAll()
        {
            try
            {
                return new List<DeviceInfo>(_routerApiConsumer.GetAll());
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FEJL DETALJE] " + ex.ToString());
                throw new Exception("Fejl ved hentning af enheder fra router", ex);
            }
        }
    }
}
