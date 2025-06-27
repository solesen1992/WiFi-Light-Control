using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEXO_Projekt_DAL.Model
{
    public class MACAdressDevice
    {
        public string macAdress {  get; set; }

        public MACAdressDevice(string macAdress)
        {
            this.macAdress = macAdress;
        }
    }
}
