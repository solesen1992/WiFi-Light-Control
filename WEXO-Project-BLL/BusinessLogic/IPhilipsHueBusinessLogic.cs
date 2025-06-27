using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    public interface IPhilipsHueBusinessLogic
    {
        IList<Light> GetAll();
        void TurnOffLight(int id);
        void TurnOnLight(int id);


    }
}
