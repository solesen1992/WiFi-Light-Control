using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Model for philips hue light
 */

namespace WEXO_Projekt_DAL.Model
{
    public class Light
    {
        public string Id { get; set; }

        [JsonProperty("state")]
        public LightState State { get; set; }

        [JsonIgnore]
        public int ParsedId => int.TryParse(Id, out var parsed) ? parsed : -1;
    }
}
