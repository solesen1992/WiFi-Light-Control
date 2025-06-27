using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Wrapper class that can get the information.
 * Router information - takes all the top-level json information and handles that.
 */

namespace WEXO_Test_Sorting_Blacklist.Model.HelperClass
{
    public class RouterApiResponse<T>
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public string ResponseId { get; set; }
        public string Message { get; set; }
        public T Data { get; set; } // Contains the data about all the devices
    }
}
