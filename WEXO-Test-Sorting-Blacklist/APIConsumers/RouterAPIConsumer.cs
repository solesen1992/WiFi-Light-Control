using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEXO_Test_Sorting_Blacklist.Model;
using WEXO_Test_Sorting_Blacklist.Model.HelperClass;

namespace WEXO_Test_Sorting_Blacklist.APIConsumers
{
    /**
     * Consumer class responsible for making API calls to the router
     * to fetch active WiFi device information.
     * Handles REST requests and SSL certificate validation.
     */
    public class RouterAPIConsumer
    {
        private readonly string baseURIforWiFi = ""; /* INSERT YOUR OWN API BASE URL FOR WIFI*/
        private RestClient restClient;

        public RouterAPIConsumer()
        {
            restClient = new RestClient(baseURIforWiFi);
        }

        /**
         * Retrieves a list of active WiFi devices from the router.
         * Applies custom SSL certificate validation to accept specific self-signed certificates.
         * Throws an exception if the API request is unsuccessful.
         *
         * @return IList<DeviceInfo> - list of devices currently active on the WiFi network.
         * @throws Exception - when API request fails or data cannot be retrieved.
         */
        public IList<DeviceInfo> GetWiFiDevices()
        {
            try
            {
                var optionsForWiFi = new RestClientOptions(baseURIforWiFi)
                {
                    RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                    {
                        if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                            return true; 

                        if (cert is System.Security.Cryptography.X509Certificates.X509Certificate2 cert2)
                        {
                            if (cert2.Subject.Contains("CN=") || cert2.Thumbprint == "") /*INSERT YOUR OWN CN AND THUMBPRINT*/
                            {
                                return true; 
                            }
                        }

                        return false; 
                    }
                };

                var clientForWiFi = new RestClient(optionsForWiFi);


                var request = new RestRequest("", Method.Get);
                request.AddHeader("Accept", "application/json");
                request.AddHeader("x-api-key", ""); /* INSERT YOUR OWN API KEY */

                var responseForWiFi = clientForWiFi.Execute(request);



                if (!responseForWiFi.IsSuccessful)
                {
                    throw new Exception($"Request failed: {responseForWiFi.StatusCode} - {responseForWiFi.ErrorMessage}");
                }

                var apiOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var apiResponseForWiFi = JsonSerializer.Deserialize<RouterApiResponse<List<DeviceInfo>>>(responseForWiFi.Content, apiOptions);

                var result = new List<DeviceInfo>();

                foreach (var item in apiResponseForWiFi.Data)
                {
                    result.Add(item);
                }

                return result;

            }
            catch (Exception e)
            {
                throw new Exception($"Could not retrieve LAN data. Error: {e.Message}", e);
            }
        }

    }
}
