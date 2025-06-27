using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Projekt_DAL.DB
{
    public interface ISettingsDB
    {
        SettingsConfiguration GetSettings();

        void SaveSettings(SettingsConfiguration settings);
    }
}
