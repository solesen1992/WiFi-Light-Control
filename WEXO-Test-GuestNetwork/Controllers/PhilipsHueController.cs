using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.APIConsumers;

namespace WEXO_Test_GuestNetwork.Controllers
{
    /**
     * Controller responsible for managing Philips Hue light states via API communication.
     * Implements retry logic for reliability, and ensures lights are only toggled when necessary.
     */
    public class PhilipsHueController
    {
        private readonly PhilipsHueAPIConsumer _philipsHueApiConsumer;

        public PhilipsHueController()
        {
            _philipsHueApiConsumer = new PhilipsHueAPIConsumer();
        }

        /**
         * Attempts to turn off the light with the specified ID.
         * Verifies current light status before acting. If the light is already off, no action is taken.
         * Retries up to 5 times if the command doesn't succeed immediately.
         *
         * @param id The ID of the Philips Hue light to turn off.
         */
        public void TurnOffLight(int id)
        {
            try
            {
                var isLightOn = _philipsHueApiConsumer.GetLightStatus(id);
                if (!isLightOn)
                {
                    Console.WriteLine($"[DEBUG] Lys {id} er allerede slukket.");
                    return;
                }

                Console.WriteLine($"[DEBUG] Forsøger at SLUKKE lys med ID {id}");
                _philipsHueApiConsumer.TurnOffLightDirect(id);

                
                int attempts = 0;
                while (attempts < 5) 
                {
                    var newStatus = _philipsHueApiConsumer.GetLightStatus(id);
                    if (!newStatus)
                    {
                        Console.WriteLine($"[DEBUG] Lys {id} er nu slukket.");
                        break;
                    }
                    else
                    {
                        attempts++;
                        Console.WriteLine($"[DEBUG] Lys {id} er stadig tændt. Forsøger igen ({attempts}/5).");
                        System.Threading.Thread.Sleep(2000); 
                    }
                }
           
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

        /**
         * Attempts to turn on the light with the specified ID.
         * Verifies current light status before acting. If the light is already on, no action is taken.
         * Retries up to 5 times if the command doesn't succeed immediately.
         *
         * @param id The ID of the Philips Hue light to turn on.
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
                    return; 
                }

                Console.WriteLine($"[DEBUG] Forsøger at TÆNDE lys med ID {id}");
                _philipsHueApiConsumer.TurnOnLightDirect(id);

                int attempts = 0;
                while (attempts < 5) 
                {
                    var newStatus = _philipsHueApiConsumer.GetLightStatus(id);
                    if (newStatus) 
                    {
                        Console.WriteLine($"[DEBUG] Lys {id} er nu tændt.");
                        break;
                    }
                    else
                    {
                        attempts++;
                        Console.WriteLine($"[DEBUG] Lys {id} er stadig slukket. Forsøger igen ({attempts}/5).");
                        System.Threading.Thread.Sleep(2000);
                    }
                }

                if (attempts == 5)
                {
                    Console.WriteLine($"[FEJL] Lys {id} kunne ikke tændes efter flere forsøg.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FEJL] Kunne ikke tænde lys {id}: {ex.Message}");
            }
        }
    }
}
