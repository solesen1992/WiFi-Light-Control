using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.DB
{
    /**
     * Handles database operations for system settings.
     * Retrieves and updates settings such as system on/off status,
     * active time spans for weekdays/weekends, timeout durations, etc.
     */
    public class SettingsDB
    {
        private static string connectionString = "Server=;Database=;User Id=;Password=;TrustServerCertificate="; /* INSERT YOUR OWN DATABASE CONNECTIONSTRING */

        /**
         * Retrieves the current settings from the database.
         * If no settings are found, returns default fallback settings.
         *
         * @return SettingsConfiguration object with current or default settings.
         */
        public SettingsConfiguration GetSettings()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT TOP 1 
                            SystemOnOff,
                            WeekdayStartTime,
                            WeekdayEndTime,
                            WeekendStartTime,
                            WeekendEndTime,
                            OfflineTimeoutMinutes,
                            ManualOverrideDurationMinutes
                        FROM dbo.Settings";

                connection.Open();
                var settings = connection.QueryFirstOrDefault<SettingsConfiguration>(sql);

                if (settings == null)
                {
                    // Fallback-settings if nothing was found in the database
                    return new SettingsConfiguration
                    {
                        SystemOnOff = true, // Or false if we don't want the system to run and just turn off
                        WeekdayStartTime = TimeSpan.FromHours(6),         // 06:00
                        WeekdayEndTime = TimeSpan.FromHours(24).Subtract(TimeSpan.FromSeconds(1)), // 23:59:59
                        WeekendStartTime = TimeSpan.FromHours(6),         // 06:00
                        WeekendEndTime = TimeSpan.FromHours(24).Subtract(TimeSpan.FromSeconds(1)),  // 23:59:59
                        OfflineTimeoutMinutes = 5, // timeout in minutes before lights turn off
                        ManualOverrideDurationMinutes = 10 // manual override duration in minutes
                    };
                }

                return settings;
            }
        }

        /**
         * Saves updated settings to the database.
         * @param settings The SettingsConfiguration object containing updated values.
         */
        public void SaveSettings(SettingsConfiguration settings)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"UPDATE Settings SET
                        SystemOnOff = @SystemOnOff,
                        WeekdayStartTime = @WeekdayStartTime,
                        WeekdayEndTime = @WeekdayEndTime,
                        WeekendStartTime = @WeekendStartTime,
                        WeekendEndTime = @WeekendEndTime,
                        OfflineTimeoutMinutes = @OfflineTimeoutMinutes,
                        ManualOverrideDurationMinutes = @ManualOverrideDurationMinutes";

                connection.Open();
                connection.Execute(sql, settings);
            }
        }
    }
}
