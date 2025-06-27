using System.Linq;
using WEXO_Test_Sorting_Blacklist.Controllers;

/**
 * This test connects the BlackList and Router information and sorts it in the SortingController.
 * This test is just to show us whether we have removed all the necessary information.
 */

namespace WEXO_Test_Sorting_Blacklist
{
    public class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"\n⏰ Opdatering: {DateTime.Now}");

                try
                {
                    var sortingController = new SortingController();
                    var sortedDevices = sortingController.GetDevicesAndSort();

                    Console.WriteLine("\n📶 Filtrerede og aktive Wi-Fi-enheder:\n");

                    foreach (var d in sortedDevices.OrderBy(d => d.ip))
                    {
                        Console.WriteLine($"🔹 Hostname: {d.hostname ?? "(ukendt)"} | IP: {d.ip} | MAC: {d.mac} | Status: {d.active_status} | Descr: {d.descr}");
                    }

                    Console.WriteLine($"\n🔢 Antal enheder fundet: {sortedDevices.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"💥 Fejl ved hentning af enheder: {ex.Message}");
                }

                Console.WriteLine(new string('-', 80)); // Separator mellem opdateringer

                Thread.Sleep(60000); // 1 minut
            }
        }
    }
}
