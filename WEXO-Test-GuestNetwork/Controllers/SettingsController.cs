using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.DB;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.Controllers
{
    /**
     * Controller responsible for retrieving and updating system configuration settings.
     * Provides methods to toggle system state, manage active time ranges, and set timeout values.
     * Interacts directly with SettingsDB for persistent configuration storage.
     */
    public class SettingsController
    {
        private readonly SettingsDB _settingsDB;


        public SettingsController()
        {
            _settingsDB = new SettingsDB();
        }

        /**
         * Retrieves the current system settings from the database.
         * @return A SettingsConfiguration object representing the current settings.
         */
        public SettingsConfiguration GetCurrentSettings()
        {
            return _settingsDB.GetSettings();
        }

        /**
         * Toggles the overall system status (on/off) and saves the updated state.
         */
        public void ToggleSystemStatus()
        {
            var settings = GetCurrentSettings();
            settings.SystemOnOff = !settings.SystemOnOff;
            _settingsDB.SaveSettings(settings);
        }

        /**
         * Checks whether the current time falls within the configured active time period.
         * Considers weekday vs weekend times depending on the current day.
         * @return True if current time is within allowed active hours, otherwise false.
         */
        public bool IsWithinActiveTime()
        {
            var settings = GetCurrentSettings();
            var now = DateTime.Now;
            var currentTime = now.TimeOfDay;
            var day = now.DayOfWeek;

            Console.WriteLine($"[DEBUG] Klokken nu: {now:T}");
            Console.WriteLine($"[DEBUG] Weekday start: {settings.WeekdayStartTime}, slut: {settings.WeekdayEndTime}");
            Console.WriteLine($"[DEBUG] Weekend start: {settings.WeekendStartTime}, slut: {settings.WeekendEndTime}");

            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Friday)
            {
                // Weekday
                bool isWithin = currentTime >= settings.WeekdayStartTime && currentTime <= settings.WeekdayEndTime;
                Console.WriteLine($"[DEBUG] Weekday check result: {isWithin}");
                return isWithin;
            }
            else
            {
                // Weekend
                bool isWithin = currentTime >= settings.WeekendStartTime && currentTime <= settings.WeekendEndTime;
                Console.WriteLine($"[DEBUG] Weekend check result: {isWithin}");
                return isWithin;
            }
        }

        /**
         * Sets the active time range for weekdays.
         * @param start Start time of the active period.
         * @param end End time of the active period.
         */
        public void SetWeekdayTimeSpan(TimeSpan start, TimeSpan end)
        {
            var settings = GetCurrentSettings();
            settings.WeekdayStartTime = start;
            settings.WeekdayEndTime = end;
            _settingsDB.SaveSettings(settings);
        }

        /**
         * Sets the active time range for weekends.
         * @param start Start time of the active period.
         * @param end End time of the active period.
         */
        public void SetWeekendTimeSpan(TimeSpan start, TimeSpan end)
        {
            var settings = GetCurrentSettings();
            settings.WeekendStartTime = start;
            settings.WeekendEndTime = end;
            _settingsDB.SaveSettings(settings);
        }

        /**
         * Updates the offline timeout period that determines how quickly lights turn off
         * after devices go offline.
         * @param minutes Timeout duration in minutes.
         */
        public void SetOfflineTimeout(int minutes)
        {
            var settings = GetCurrentSettings();
            settings.OfflineTimeoutMinutes = minutes;
            _settingsDB.SaveSettings(settings);
        }

        /**
         * Persists all settings changes using the provided updated configuration object.
         * @param updatedSettings The modified SettingsConfiguration object to save.
         */
        public void SaveAll(SettingsConfiguration updatedSettings)
        {
            _settingsDB.SaveSettings(updatedSettings);
        }

    }
}
