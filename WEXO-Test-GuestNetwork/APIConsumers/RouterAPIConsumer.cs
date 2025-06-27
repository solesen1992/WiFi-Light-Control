using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.Model;
using WEXO_Test_GuestNetwork.Model.HelperClass;

/**
 * RouterAPIConsumer
 * 
 * This class is responsible for communicating with the router's API to fetch connected devices.
 * It handles WiFi device data and uses custom certificate validation
 * to safely connect to a known trusted router (e.g., pfSense) even with self-signed certificates.
 * The data fetched is used to determine which devices are currently online and active.
 */

namespace WEXO_Test_GuestNetwork.APIConsumers
{
    public class RouterAPIConsumer
    {
        private readonly string baseURIforWiFi = ""; /* INSERT YOUR OWN API FOR YOUR WIFI*/
        private RestClient restClient;



        /**
         * Helper Method: CreateRestClientForWiFi
         * 
         * Creates and configures a RestClient specifically for fetching WiFi device data. 
         * This method ensures proper SSL certificate validation against a known router's certificate.
         * It avoids issues with self-signed certificates by checking for an expected certificate thumbprint or subject.
         * 
         * @return A configured RestClient ready for use with the WiFi API endpoint.
         */
        private RestClient CreateRestClientForWiFi()
        {
            var optionsForWiFi = new RestClientOptions(baseURIforWiFi)
            {
                RemoteCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                        return true; // No errors, certificate is valid

                    // Validate the certificate's thumbprint or subject to ensure it matches the expected router certificate
                    if (cert is System.Security.Cryptography.X509Certificates.X509Certificate2 cert2)
                    {
                        // Only accept this specific certificate (e.g., pfSense certificate)
                        if (cert2.Subject.Contains("CN=") || cert2.Thumbprint == "") /* INSERT YOUR OWN CN AND THUMBPRINT */
                        {
                            return true;
                        }
                    }

                    return false; // Reject all other certificates
                }
            };

            return new RestClient(optionsForWiFi);
        }

        /**
         * Helper Method: CreateRequestForWiFi
         * 
         * Creates and configures a GET request for the WiFi API to fetch connected device data.
         * This method ensures the correct headers (Accept and API Key) are added for the request.
         * 
         * @return A configured RestRequest with the necessary headers.
         */
        private RestRequest CreateRequestForWiFi()
        {
            var request = new RestRequest("", Method.Get);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("x-api-key", ""); /* INSERT YOUR OWN API KEY*/

            return request;
        }

        /**
         * GetWiFiDevices
         * 
         * Fetches the devices connected to the WiFi network by making a GET request to the router's API.
         * The request uses the custom RestClient with SSL validation, handles errors, and processes the response.
         * The response is deserialized into a list of DeviceInfo objects, which represent each connected device.
         * 
         * @return A list of DeviceInfo objects representing the devices connected to the WiFi network.
         */
        public IList<DeviceInfo> GetWiFiDevices()
        {
            try
            {
                // Create the RestClient for WiFi with SSL certificate validation
                var clientForWiFi = CreateRestClientForWiFi();

                // Create the request for the WiFi API with necessary headers
                var request = CreateRequestForWiFi();

                // Execute the request and get the response
                var responseForWiFi = clientForWiFi.Execute(request);

                if (!responseForWiFi.IsSuccessful)
                {
                    throw new Exception($"Request failed: {responseForWiFi.StatusCode} - {responseForWiFi.ErrorMessage}");
                }

                // Deserialize the response content into a list of DeviceInfo objects
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
                throw new Exception($"Could not retrieve WiFi data. Error: {e.Message}", e);
            }
        }
    }
}
