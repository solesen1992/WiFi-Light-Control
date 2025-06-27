using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

/**
 * PhilipsHueAPIConsumer
 * 
 * This class serves as a client for communicating with the Philips Hue REST API.
 * It abstracts HTTP logic behind simple method calls to get light information and control lights.
 * Using RestSharp, it sends GET and PUT requests to control light states (on/off) or retrieve data.
 * This design helps separate concerns: Other parts of the system can use this class without worrying about the API details.
 */
namespace WEXO_Test_GuestNetwork.APIConsumers
{
    public class PhilipsHueAPIConsumer
    {
        private string baseURI = ""; /* INSERT YOUR OWN PHILIPS HUE API URI*/
        private RestClient restClient;

        /**
         * Constructor
         * Initializes the RestClient using the Philips Hue API base URI.
         * This allows the instance to reuse the same client for multiple requests.
         */
        public PhilipsHueAPIConsumer()
        {
            restClient = new RestClient(baseURI);
        }

        /**
         * TurnOnLightDirect
         * Sends a PUT request to explicitly turn on a light, without checking current state.
         * This is more reliable than toggling, especially if the light's state is out of sync.
         * Throws an exception if the API call fails.
         */
        public void TurnOnLightDirect(int id)
        {
            var client = new RestClient($"{baseURI}/{id}/state");

            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddJsonBody(new { on = true });

            var response = client.ExecutePut(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to turn on light {id}: {response.Content}");
            }
        }

        /**
         * TurnOffLightDirect
         * Sends a PUT request to explicitly turn off a light, without relying on state checks.
         * Useful for ensuring the light is turned off no matter what the current state says.
         * Throws an exception if the API call fails.
         */
        public void TurnOffLightDirect(int id)
        {
            var client = new RestClient($"{baseURI}/{id}/state");

            var request = new RestRequest();
            request.Method = Method.Put;
            request.AddJsonBody(new { on = false });

            var response = client.ExecutePut(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Failed to turn off light {id}: {response.Content}");
            }
        }

        /** 
         * This method retrieves the light status from a specified endpoint using a REST API.
         * It makes a GET request to retrieve the status of a light based on the provided id.
         * 
         * @param id The unique identifier of the light to get the status for.
         * @return A boolean indicating whether the light is on (true) or off (false).
         */
        public bool GetLightStatus(int id)
        {
            var client = new RestClient($"{baseURI}/{id}");

            var request = new RestRequest();
            request.Method = Method.Get;

            var response = client.ExecuteGet(request);
            if (response.IsSuccessful)
            {
                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);

                if (jsonResponse != null && jsonResponse.ContainsKey("state"))
                {
                    var state = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonResponse["state"].ToString());
                    if (state.ContainsKey("on"))
                    {
                        return (bool)state["on"];
                    }
                }
            }
            else
            {
                Console.WriteLine($"[FEJL] Kunne ikke hente status for lys {id}: {response.Content}");
            }
            return false;
        }
    }
}
