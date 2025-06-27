using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WEXO_Test_GuestNetwork.Controllers;
using WEXO_Test_GuestNetwork.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using WEXO_Test_GuestNetwork.Model;
using Microsoft.Extensions.Hosting;


namespace WEXO_Test_GuestNetwork
{
    public class Program
    {
        static void Main(string[] args)
        {
            var settingsController = new SettingsController();

            var currentSettings = settingsController.GetCurrentSettings();
            Console.WriteLine("[TEST] Henter indstillinger fra databasen: " +
                $"Weekday fra {currentSettings.WeekdayStartTime} til {currentSettings.WeekdayEndTime}");


            var controller = new MainController();
            controller.StartMonitoringLoop(checkIntervalSeconds: 10);
        }
    }
}
