using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Project_BLL.BusinessLogic;
using WEXO_Project_BLL.Facade;
using WEXO_Project_Start_Program_Here;
using WEXO_Projekt_DAL.APIConsumers;
using WEXO_Projekt_DAL.DB;
using WEXO_Projekt_DAL.Model.Helpers;

namespace WEXO_Project_BLL
{
    /*
     * Program.cs serves as the entry point for the backend console application.
     * It configures and initializes dependency injection for both the DAL and BLL layers.
     *
     * - Loads configuration values from appsettings.json.
     * - Registers dependencies such as database access, router API, and Philips Hue API.
     * - Builds a service provider and starts the MainFacade, which manages the system's monitoring and automation logic.
     */
    public class Program
    {
        static void Main(string[] args)
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure path is correct
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var services = new ServiceCollection();

            //IConfiguation depedency injection used in DAL classes
            services.AddSingleton<IConfiguration>(config);

            //DAL Layer Injections
            services.AddScoped<ISettingsDB, SettingsDB>();
            services.AddScoped<IBlackListDB, BlackListDB>();
            services.AddScoped<IRouterAPIConsumer, RouterAPIConsumer>();
            services.AddScoped<IPhilipsHueAPIConsumer, PhilipsHueAPIConsumer>();

            //BLL Layer Injections
            services.AddScoped<IBlackListBusinessLogic, BlackListBusinessLogic>();
            services.AddScoped<ISettingsBusinessLogic, SettingsBusinessLogic>();
            services.AddScoped<IRouterBusinessLogic, RouterBusinessLogic>();
            services.AddScoped<IPhilipsHueBusinessLogic, PhilipsHueBusinessLogic>();
            services.AddScoped<ISortingBusinessLogic, SortingBusinessLogic>();

            //MainFacade Layer Injection
            services.AddScoped<IMainFacade, MainFacade>();

            var serviceProvider = services.BuildServiceProvider();

            var settingsController = serviceProvider.GetRequiredService<ISettingsBusinessLogic>();

            // Get current settings from database
            var currentSettings = settingsController.GetCurrentSettings();
            Console.WriteLine("[TEST] Henter indstillinger fra databasen: " +
                $"Weekday fra {currentSettings.WeekdayStartTime} til {currentSettings.WeekdayEndTime}");


            var controller = serviceProvider.GetRequiredService<IMainFacade>();
            controller.StartMonitoringLoop(checkIntervalSeconds: 10);
        }
    }
}
