using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;

namespace WEXO_Project_BLL.BusinessLogic
{
    public interface ISettingsBusinessLogic
    {
        SettingsConfiguration GetCurrentSettings();
        void ToggleSystemStatus();
        bool IsWithinActiveTime();
        void SetWeekdayTimeSpan(TimeSpan start, TimeSpan end);
        void SetWeekendTimeSpan(TimeSpan start, TimeSpan end);
        void SetOfflineTimeout(int minutes);
        void SaveAll(SettingsConfiguration updatedSettings);

    }
}
