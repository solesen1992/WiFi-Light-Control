using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.DB;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    /*
     * Business logic for managing system settings related to lighting.
     * This class interacts with the DAL layer to retrieve and update settings,
     * including active time periods, system on/off state, and timeout settings.
     */
    public class SettingsBusinessLogic : ISettingsBusinessLogic
    {
        private readonly ISettingsDB _settingsDB;

        public SettingsBusinessLogic(ISettingsDB settingsDB)
        {
            _settingsDB = settingsDB;
        }

        /*
         * Retrieves the current system configuration from the data layer.
         * Returns the full SettingsConfiguration object with all settings.
         */
        public SettingsConfiguration GetCurrentSettings()
        {
            return _settingsDB.GetSettings();
        }

        /*
         * Toggles the current system status (on/off).
         * Useful for enabling or disabling the system without changing other settings.
         */
        public void ToggleSystemStatus()
        {
            var settings = GetCurrentSettings();
            settings.SystemOnOff = !settings.SystemOnOff;
            _settingsDB.SaveSettings(settings);
        }

        /*
         * Checks whether the current time falls within the configured active time span
         * for either weekdays or weekends. It handles both normal and overnight time spans,
         * where the end time may be earlier than the start time (e.g., 22:00–06:00, crossing midnight).
         * Returns true if the system should be considered active at the current time.
         */
        public bool IsWithinActiveTime()
        {
            var settings = GetCurrentSettings();
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var day = now.DayOfWeek;

            Console.WriteLine($"[DEBUG] Klokken nu: {now:T}");

            TimeSpan start, end;
            bool isWeekday = day >= DayOfWeek.Monday && day <= DayOfWeek.Friday;

            if (isWeekday)
            {
                start = settings.WeekdayStartTime;
                end = settings.WeekdayEndTime;
                Console.WriteLine($"[DEBUG] Weekday check - Start: {start}, End: {end}");
            }
            else
            {
                start = settings.WeekendStartTime;
                end = settings.WeekendEndTime;
                Console.WriteLine($"[DEBUG] Weekend check - Start: {start}, End: {end}");
            }

            bool isWithin;

            if (start <= end)
            {
                // Normal case (e.g., 08:00–18:00)
                isWithin = currentTime >= start && currentTime <= end;
            }
            else
            {
                // Overnight case (e.g., 22:00–06:00)
                isWithin = currentTime >= start || currentTime <= end;
            }

            Console.WriteLine($"[DEBUG] Current time: {currentTime}, Within active time: {isWithin}");
            return isWithin;
        }

        /*
         * Updates the configured active time span for weekdays.
         * Takes a start and end TimeSpan to define when the system should be active.
         */
        public void SetWeekdayTimeSpan(TimeSpan start, TimeSpan end)
        {
            var settings = GetCurrentSettings();
            settings.WeekdayStartTime = start;
            settings.WeekdayEndTime = end;
            _settingsDB.SaveSettings(settings);
        }

        /*
         * Updates the configured active time span for weekends.
         * Takes a start and end TimeSpan to define weekend activity hours.
         */
        public void SetWeekendTimeSpan(TimeSpan start, TimeSpan end)
        {
            var settings = GetCurrentSettings();
            settings.WeekendStartTime = start;
            settings.WeekendEndTime = end;
            _settingsDB.SaveSettings(settings);
        }

        /*
         * Sets the timeout duration for when lights should automatically turn off 
         * if no devices are detected online.
         * The duration is set in minutes.
         */
        public void SetOfflineTimeout(int minutes)
        {
            var settings = GetCurrentSettings();
            settings.OfflineTimeoutMinutes = minutes;
            _settingsDB.SaveSettings(settings);
        }

        /*
         * Saves a complete settings configuration at once.
         * Useful for when the user makes multiple changes and clicks "Save All".
         */
        public void SaveAll(SettingsConfiguration updatedSettings)
        {
            _settingsDB.SaveSettings(updatedSettings);
        }
    }

}
