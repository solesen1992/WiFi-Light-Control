using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Model for devices such as phone, pc etc. that is on the network.
 */

namespace WEXO_Projekt_DAL.Model
{
    public class DeviceInfo
    {
        public int id { get; set; }
        public string ip { get; set; }
        public string mac { get; set; }
        public string hostname { get; set; }
        public string starts { get; set; }
        public string ends { get; set; }
        public string active_status { get; set; }
        public string online_status { get; set; }
        public string descr { get; set; }

        public override string ToString()
        {
            return $"id = {id}, ip = {ip}, mac = {mac}, hostname = {hostname}, starts = {starts}, ends = {ends}, active_status = {active_status}, online_status = {online_status}, descr = {descr}";
        }
    }
}
