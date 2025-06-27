using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEXO_Project_Start_Program_Here
{
 /**
 * RouterAPIDependencyInjection
 * 
 * This class serves as a data container for the configuration needed to interact with the router's API.
 * It holds necessary information like the WiFi base URI, API key, and certificate validation details.
 * It is primarily used to pass these configuration details to classes that need to connect to the router's API.
 */
    public class RouterAPIDependencyInjection
    {
        public string WiFiBaseUri { get; set; }
        public string ApiKey { get; set; }
        public string AllowedCertificateCommonName { get; set; }
        public string AllowedCertificateThumbprint { get; set; }

        /**
         * Constructor
         * 
         * Initializes a new instance of the RouterAPIDependencyInjection class with the provided configuration values.
         * These values are necessary for connecting securely to the router's API, including the WiFi URI, API key,
         * and certificate details for validation.
         * 
         * @param wiFiBaseUri The base URI for the router's WiFi API.
         * @param apiKey The API key used to authenticate requests.
         * @param allowedCertificateCommonName The common name of the certificate used for SSL validation.
         * @param allowedCertificateThumbprint The thumbprint of the certificate used for SSL validation.
         */
        public RouterAPIDependencyInjection(string wiFiBaseUri, string apiKey, string allowedCertificateCommonName, string allowedCertificateThumbprint)
        {
            WiFiBaseUri = wiFiBaseUri;
            ApiKey = apiKey;
            AllowedCertificateCommonName = allowedCertificateCommonName;
            AllowedCertificateThumbprint = allowedCertificateThumbprint;
        }
    }
}
