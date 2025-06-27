using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.Controllers
{
    /**
     * Main controller responsible for monitoring device presence on the network
     * and controlling Philips Hue lights accordingly. It integrates sorting logic,
     * lighting control, and user-configurable settings to control lights behavior.
     */
    public class MainController
    {
        private readonly SortingController _sortingController;
        private readonly PhilipsHueController _philipsHueController;
        private readonly SettingsController _settingsController;
        private DateTime? _wentOfflineAt;
        private bool _wasOnlineLastTime;
        private bool _hasInitializedStatus = false;

        public MainController()
        {
            _sortingController = new SortingController();
            _philipsHueController = new PhilipsHueController();
            _settingsController = new SettingsController();
            _wasOnlineLastTime = false;
            _wentOfflineAt = null;
        }

        /**
         * This method applies lighting logic based on presence of devices.
         * Turns lights on if devices are present; otherwise turns them off.
         */
        public void LightsControlWithWiFi(List<DeviceInfo> devices)
        {
   
            Console.WriteLine($"[DEBUG] Aktivt tidsrum fra {_settingsController.GetCurrentSettings().WeekdayStartTime} til {_settingsController.GetCurrentSettings().WeekdayEndTime}");
            Console.WriteLine($"[DEBUG] Aktuel tid: {DateTime.Now.TimeOfDay}");

            Console.WriteLine($"[DEBUG] LightsControlWithWiFi kaldt med {devices.Count} enhed(er)");

            bool isWithinActiveTime = _settingsController.IsWithinActiveTime();
            Console.WriteLine(isWithinActiveTime
                ? "[INFO] Vi er INDENFOR aktivt tidsrum"
                : "[INFO] Vi er UDENFOR aktivt tidsrum");

            if (devices == null || devices.Count == 0)
            {
                Console.WriteLine("[DEBUG] Ingen enheder fundet – forsøger at SLUKKE lys");
                _philipsHueController.TurnOffLight(40);
                _philipsHueController.TurnOffLight(39);
            }
            else
            {
                Console.WriteLine("[DEBUG] Enheder fundet – forsøger at TÆNDE lys");
                _philipsHueController.TurnOnLight(40);
                _philipsHueController.TurnOnLight(39);
                Console.WriteLine("[DEBUG] Lys skulle nu være tændt – hvis API virker.");
            }
        }

        /**
         * StartMonitoringLoop
         * 
         * Starts a continuous loop that monitors the devices on the network at regular intervals. 
         * It checks whether the system is online or offline and handles the status accordingly.
         * If the system is online and within active time, it monitors devices and controls the lights.
         * If the system is offline or outside of active time, it handles these cases appropriately.
         * 
         * @param checkIntervalSeconds The interval (in seconds) at which the devices will be monitored.
         */
        public void StartMonitoringLoop(int checkIntervalSeconds = 10)
        {
            Console.WriteLine($"Starter overvågning af devices på netværket hvert {checkIntervalSeconds} sekund...\n");

            while (true)
            {
                try
                {
                    var currentSettings = _settingsController.GetCurrentSettings();
                    Console.WriteLine($"[STATUS] SystemOnOff: {(currentSettings.SystemOnOff ? "ON" : "OFF")}");

                    if (!currentSettings.SystemOnOff)
                    {
                        Console.WriteLine("[INFO] Systemet er slået fra – overvågning springes over.");
                        _wasOnlineLastTime = false;
                    }
                    else
                    {
                        
                        if (!_hasInitializedStatus && _settingsController.IsWithinActiveTime())
                        {
                            InitStatus();
                            _hasInitializedStatus = true;
                        }

                        var timeoutMinutes = currentSettings.OfflineTimeoutMinutes;
                        var timeout = TimeSpan.FromMinutes(timeoutMinutes);

                        Console.WriteLine($"[INFO] Overvåger med timeout på {timeoutMinutes} minutter.");
                        MonitorDevice(timeout, timeoutMinutes);
                    }

                    Thread.Sleep(checkIntervalSeconds * 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fejl under overvågning: " + ex.Message);
                    Thread.Sleep(checkIntervalSeconds * 1000);
                }
            }
        }

        /**
         * InitStatus
         * 
         * Initializes the status by checking whether any devices are online or offline at the start of monitoring.
         * It adjusts the light status accordingly based on whether any devices are online.
         */
        private void InitStatus()
        {
            var rawDevices = _sortingController.GetDevicesAndSort();
            var devices = rawDevices.Select(d => new DeviceInfo
            {
                hostname = d.hostname,
                mac = d.mac
            }).ToList();

            var isOnline = devices.Any();
            _wasOnlineLastTime = isOnline;

            if (isOnline)
            {
                Console.WriteLine($" er ONLINE ved start – tænder lys ({DateTime.Now:T})");
            }
            else
            {
                Console.WriteLine($" er OFFLINE ved start – slukker lys ({DateTime.Now:T})");
                _wentOfflineAt = DateTime.Now;
            }

            LightsControlWithWiFi(devices);
        }

        /**
         * MonitorDevice
         * 
         * Monitors the devices by checking whether they are online or offline. If devices are online, it will turn
         * on the lights; if offline, it will handle the timeout and turn off the lights if necessary.
         * 
         * @param timeout The timeout period after which the system will consider devices as offline.
         * @param timeoutMinutes The timeout period in minutes for which the system waits before turning off the lights.
         */
        private void MonitorDevice(TimeSpan timeout, int timeoutMinutes)
        {
            var rawDevices = _sortingController.GetDevicesAndSort();
            var devices = rawDevices.Select(d => new DeviceInfo
            {
                hostname = d.hostname,
                mac = d.mac
            }).ToList();

            var isOnline = devices.Any();

            if (isOnline)
            {
                Console.WriteLine($"STATUS: er ONLINE ({DateTime.Now:T})");
                HandleOnlineStatus(devices);
            }
            else
            {
                Console.WriteLine($"STATUS: Ingen enheder er online – OFFLINE ({DateTime.Now:T})");
                HandleOfflineStatus(devices, timeout, timeoutMinutes);
            }

            Console.WriteLine($" {devices.Count} enhed(er) online: {FormatDeviceList(devices)}");
        }

        /**
         * HandleOnlineStatus
         * 
         * Handles the scenario where devices are online. If the system is on and within active time, it turns on the lights
         * and resets the offline timeout counter.
         * 
         * @param devices The list of devices that are currently detected as online.
         */
        private void HandleOnlineStatus(List<DeviceInfo> devices)
        {
            var settings = _settingsController.GetCurrentSettings();

            if (!settings.SystemOnOff)
            {
                Console.WriteLine("Systemet er slået fra - ignorerer ONINE-status");
                return;
            }
            if (_settingsController.IsWithinActiveTime())
            {
                if (!_wasOnlineLastTime)
                {
                    Console.WriteLine($" er kommet ONLINE – tænder lys ({DateTime.Now:T})");
                    LightsControlWithWiFi(devices);
                    _wasOnlineLastTime = true;
                }
                _wentOfflineAt = null;
            }
            else
            {
                Console.WriteLine("Udenfor aktivt tidsrum - tænder ikke lys");
            }

        }

        /**
         * HandleOfflineStatus
         * 
         * Handles the scenario where no devices are online. It starts the timeout timer, and if the devices remain offline
         * for the defined timeout period, it turns off the lights.
         * 
         * @param devices The list of devices that are currently detected as offline.
         * @param timeout The timeout period after which the lights will be turned off if no devices are online.
         * @param timeoutMinutes The timeout period in minutes.
         */
        private void HandleOfflineStatus(List<DeviceInfo> devices, TimeSpan timeout, int timeoutMinutes)
        {
            var settings = _settingsController.GetCurrentSettings();

            if (!settings.SystemOnOff)
            {
                Console.WriteLine("Systemet er slået fra - ignorerer OFFLINE-status");
                return;
            }

            if (_settingsController.IsWithinActiveTime())
            {
                if (_wasOnlineLastTime && _wentOfflineAt == null)
                {
                    _wentOfflineAt = DateTime.Now;
                    Console.WriteLine($" er blevet OFFLINE – starter timeout ({DateTime.Now:T})");
                }
                if (_wentOfflineAt != null &&
                (DateTime.Now - _wentOfflineAt.Value) >= timeout &&
                _wasOnlineLastTime)
                {
                    Console.WriteLine($" har været offline i {timeoutMinutes} minutter – slukker lys ({DateTime.Now:T})");
                    LightsControlWithWiFi(devices);
                    _wasOnlineLastTime = false;
                    _wentOfflineAt = null;
                }
                else if (_wentOfflineAt != null)
                {
                    double secs = (DateTime.Now - _wentOfflineAt.Value).TotalSeconds;
                    Console.WriteLine($" stadig offline – venter ({secs:F0} sek af {timeout.TotalSeconds:F0}) ({DateTime.Now:T})");
                }
            }
            else
            {
                Console.WriteLine("Udenfor aktivt tidsrum - ignorerer OFFLINE status");
            }
        }

        /**
         * FormatDeviceList
         * 
         * Formats a list of devices into a string for console display.
         * 
         * @param devices The list of devices to format.
         * @return A formatted string representing the list of devices.
         */
        private string FormatDeviceList(List<DeviceInfo> devices)
        {
            if (devices == null || devices.Count == 0)
                return "(ingen enheder)";

            var deviceNames = devices.Select(d =>
            !string.IsNullOrWhiteSpace(d.hostname) ? d.hostname :
            "Unknown"
            );

            return string.Join(", ", deviceNames);
        }
    }
}
