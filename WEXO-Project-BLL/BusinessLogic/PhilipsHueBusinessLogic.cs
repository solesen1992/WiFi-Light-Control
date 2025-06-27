using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.APIConsumers;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    /*
    * This class provides business logic for interacting with the Philips Hue lights through 
    * the IPhilipsHueAPIConsumer. It acts as an intermediary that sends commands to turn lights 
    * on or off, retrieves their status, and handles any retry mechanisms in case of failure.
    * It does not directly communicate with the Philips Hue API but relies on the IPhilipsHueAPIConsumer.
    */
    public class PhilipsHueBusinessLogic: IPhilipsHueBusinessLogic
    {
        private readonly IPhilipsHueAPIConsumer _philipsHueApiConsumer;

        public PhilipsHueBusinessLogic(IPhilipsHueAPIConsumer philipsHueAPIConsumer)
        {
            _philipsHueApiConsumer = philipsHueAPIConsumer;
        }

        /*
         * Retrieves all the lights from the Philips Hue system using the IPhilipsHueAPIConsumer.
         * The method calls GetAll() on IPhilipsHueAPIConsumer to fetch the list of all lights.
         * 
         * Returns a list of Light objects representing all the lights in the Philips Hue system.
         * If there's an error fetching the data, an empty list is returned to prevent further failures in the system.
         */
        public IList<Light> GetAll()
        {
            try
            {
                return _philipsHueApiConsumer.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FEJL] Kunne ikke hente lys: {ex.Message}");
                return new List<Light>(); // Return an empty list to prevent failure in further operations
            }
        }

        /*
         * Turns off the light with the given ID.
         * A blacklist of certain lights is used to ensure that specific lights cannot be turned off.
         * The method also includes a retry mechanism that attempts to turn off the light up to 5 times 
         * if the initial command fails.
         * 
         * The method checks if the light is already off before attempting to turn it off.
         * If the light cannot be turned off after 5 attempts, a failure message is logged.
         */
        public void TurnOffLight(int id)
        {
            // Blacklist of lights (outdoor lights) that should not be turned off
            var blacklistedIds = new HashSet<int> { 4, 6, 7 };
            if (blacklistedIds.Contains(id))
                return;

            try
            {
                var isLightOn = _philipsHueApiConsumer.GetLightStatus(id); // Check the light's status
                if (!isLightOn) // If the light is already off
                {
                    Console.WriteLine($"[DEBUG] Lys {id} er allerede slukket.");
                    return; // Exit as there's nothing to do
                }

                Console.WriteLine($"[DEBUG] Forsøger at SLUKKE lys med ID {id}");
                _philipsHueApiConsumer.TurnOffLightDirect(id);

                // Retry mechanism: Try turning off the light again after 2 seconds if it wasn't turned off initially
                int attempts = 0;
                while (attempts < 5) // Maximum of 5 attempts
                {
                    var newStatus = _philipsHueApiConsumer.GetLightStatus(id);
                    if (!newStatus) // Light is turned off
                    {
                        Console.WriteLine($"[DEBUG] Lys {id} er nu slukket.");
                        break;
                    }
                    else
                    {
                        attempts++;
                        Console.WriteLine($"[DEBUG] Lys {id} er stadig tændt. Forsøger igen ({attempts}/5).");
                        System.Threading.Thread.Sleep(2000); // Wait for 2 seconds before retrying
                    }
                }

                // Log an error if the light could not be turned off after 5 attempts
                if (attempts == 5)
                {
                    Console.WriteLine($"[FEJL] Lys {id} kunne ikke slukkes efter flere forsøg.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FEJL] Kunne ikke slukke lys {id}: {ex.Message}");
            }
        }

        /*
         * Turns on the light with the given ID.
         * The method checks the light's status and turns it on only if it's currently off.
         * A retry mechanism is included, which attempts to turn the light on up to 5 times 
         * if the initial command fails.
         * 
         * If the light cannot be turned on after 5 attempts, an error message is logged.
         */
        public void TurnOnLight(int id)
        {
            try
            {
                Console.WriteLine($"[DEBUG] Sending command to turn on light with ID {id}");

                var isLightOn = _philipsHueApiConsumer.GetLightStatus(id);
                if (isLightOn)
                {
                    Console.WriteLine($"[DEBUG] Lys {id} er allerede tændt.");
                    return; // Exit as there's nothing to do
                }

                Console.WriteLine($"[DEBUG] Forsøger at TÆNDE lys med ID {id}");
                _philipsHueApiConsumer.TurnOnLightDirect(id);

                // Retry mechanism: Try turning the light on again after 2 seconds if it wasn't turned on initially
                int attempts = 0;
                while (attempts < 5) // Maximum of 5 attempts
                {
                    var newStatus = _philipsHueApiConsumer.GetLightStatus(id);
                    if (newStatus) // Light is turned on
                    {
                        Console.WriteLine($"[DEBUG] Lys {id} er nu tændt.");
                        break;
                    }
                    else
                    {
                        attempts++;
                        Console.WriteLine($"[DEBUG] Lys {id} er stadig slukket. Forsøger igen ({attempts}/5).");
                        System.Threading.Thread.Sleep(2000); // Wait for 2 seconds before retrying
                    }

                    // Log an error if the light could not be turned on after 5 attempts
                    if (attempts == 5)
                    {
                        Console.WriteLine($"[FEJL] Lys {id} kunne ikke tændes efter flere forsøg.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FEJL] Kunne ikke tænde lys {id}: {ex.Message}");
            }
        }
    }
}
