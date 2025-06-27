using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;
using WEXO_Projekt_DAL.Model;
using Microsoft.Extensions.Configuration;


namespace WEXO_Projekt_DAL.DB
{
    /**
     * This class handles the database operations related to system light settings.
     * Implements ISettingsDB interface to support reading and saving settings.     
     */
    public class SettingsDB : ISettingsDB
    {
        private readonly string _connectionString;

        public SettingsDB(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConnectionString");
        }

        /**
         * Retrieves the current settings from the database.
         * Returns fallback/default settings if no record is found.
         */
        public SettingsConfiguration GetSettings()
        {
            using (var connection = new SqlConnection(_connectionString))
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
                        OfflineTimeoutMinutes = 5, // e.g. 5 minutes before turning off lights
                        ManualOverrideDurationMinutes = 10 // e.g. 10 minutes of manual control before system takes over
                    };
                }

                return settings;
            }
        }

        /**
         * Saves or updates the provided system settings to the database.
         */
        public void SaveSettings(SettingsConfiguration settings)
        {
            using (var connection = new SqlConnection(_connectionString))
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
