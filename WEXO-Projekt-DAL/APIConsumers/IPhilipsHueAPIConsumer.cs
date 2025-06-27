using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Projekt_DAL.APIConsumers
{
    public interface IPhilipsHueAPIConsumer
    {
        // Metode til at hente alle lys
        IList<Light> GetAll();

        // Metode til at tænde et lys
        void TurnOnLightDirect(int id);

        // Metode til at slukke et lys
        void TurnOffLightDirect(int id);
        bool GetLightStatus(int id);
    }
}
