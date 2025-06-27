using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEXO_Test_GuestNetwork.Model
{
    public class SettingsConfiguration
    {
        public bool SystemOnOff { get; set; }

        // Timespan for weekdays (monday to friday)
        public TimeSpan WeekdayStartTime { get; set; }
        public TimeSpan WeekdayEndTime { get; set; }

        // Timespan for weekends (sunday and saturday)
        public TimeSpan WeekendStartTime { get; set; }
        public TimeSpan WeekendEndTime { get; set; }

        // How fast the light should shut off after the last person was offline
        public int OfflineTimeoutMinutes { get; set; }
        // If people manually override the light, how long should they have it?
        public int ManualOverrideDurationMinutes { get; set; }
    }
}
