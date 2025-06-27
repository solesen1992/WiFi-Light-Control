using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEXO_Projekt_DAL.Model.Helpers
{
    public class RouterAPIDependencyInjection
    {
        public string WiFiBaseUri { get; set; }
        public string ApiKey { get; set; }
        public string AllowedCertificateCommonName { get; set; }
        public string AllowedCertificateThumbprint { get; set; }

        public RouterAPIDependencyInjection(string wiFiBaseUri, string apiKey, string allowedCertificateCommonName, string allowedCertificateThumbprint)
        {
            WiFiBaseUri = wiFiBaseUri;
            ApiKey = apiKey;
            AllowedCertificateCommonName = allowedCertificateCommonName;
            AllowedCertificateThumbprint = allowedCertificateThumbprint;
        }
    }
}
