using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Projekt_DAL.Model.Helpers
{
    /**
     * Provides helper methods to parse JSON responses related to lights.
     */
    public static class LightParser
    {
        /**
         * Deserializes a JSON string representing multiple lights into a list of Light objects.
         * @param json JSON string containing light data.
         * @return List of Light objects parsed from the JSON.
         */
        public static List<Light> DeserializeLights(string json)
        {
            var lights = new List<Light>();
            var jObject = JObject.Parse(json);

            foreach (var property in jObject.Properties())
            {
                var light = new Light
                {
                    Id = property.Name,
                    State = property.Value["state"]?.ToObject<LightState>()
                };
                lights.Add(light);
            }

            return lights;
        }
    }
}
