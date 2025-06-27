using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WEXO_Test_GuestNetwork.Model;

namespace WEXO_Test_GuestNetwork.DB
{
    /**
     * Handles database operations related to device blacklisting.
     * Supports blacklisting by MAC address, hostname, and device description.
     * Uses Dapper for database access and maps results to strongly typed models.
     */
    public class BlackListDB
    {
        private readonly string _connectionString;

        public BlackListDB()
        {
            _connectionString = "Server=;Database=;User Id=;Password=;TrustServerCertificate="; /* INSERT YOUR OWN DATABASE CONNECTIONSTRING */
        }

        /**
         * Retrieves all blacklisted MAC addresses from the database.
         * @return List of MACAdressDeviceResponseFromDB entries.
         */
        public List<MACAdressDeviceResponseFromDB> GetBlacklistedMACs()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT MAC FROM dbo.Blacklist";
                return conn.Query<MACAdressDeviceResponseFromDB>(query).ToList();
            }
        }

        /**
         * Retrieves all blacklisted hostnames from the database.
         * @return List of HostnameDevice entries.
         */
        public List<HostnameDevice> GetBlacklistedHostnames()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT Hostname FROM dbo.BlacklistHostname";
                return conn.Query<HostnameDevice>(query).ToList();
            }
        }

        /**
         * Retrieves all blacklisted device descriptions from the database.
         * @return List of DeviceDescriptionResponseFromDB entries.
         */
        public List<DeviceDescriptionResponseFromDB> GetBlackListedDescriptions()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "Select deviceDescription FROM dbo.BlacklistDescription";
                var result = conn.Query<DeviceDescriptionResponseFromDB>(query).ToList();

                return result;
            }
        }

        /**
         * Inserts a new device description into the blacklist.
         * @param obj A DeviceDescription object.
         */
        public void CreateDescription(DeviceDescription obj)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "insert into BlacklistDescription (deviceDescription) values (@description)";
                conn.Execute(query, obj);
            }
            catch (Exception e)
            {
                throw new Exception($"Error with inserting description into database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * Deletes the first matching device description from the blacklist.
         * @param description The description to delete.
         */
        public void DeleteDescription(string description)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "DELETE TOP (1) from BlacklistDescription where deviceDescription = @description";
                conn.Execute(query, new
                {
                    description = description
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error with inserting description into database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * Deletes the first matching hostname from the blacklist.
         * @param hostname The hostname to delete.
         */
        public void DeleteHostname(string hostname)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "DELETE TOP (1) from BlacklistHostname where Hostname = @hostname";
                conn.Execute(query, new
                {
                    hostname = hostname
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error with deleting hostname in database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * Inserts a new hostname into the blacklist.
         * @param obj A HostnameDevice object.
         */
        public void CreateHostname(HostnameDevice obj)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "insert into BlacklistHostname (Hostname) values (@hostName)";
                conn.Execute(query, obj);
            }
            catch (Exception e)
            {
                throw new Exception($"Error with inserting description into database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * Inserts a new MAC address into the blacklist.
         * @param obj A MACAdressDevice object.
         */
        public void CreateMAC(MACAdressDevice obj)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "insert into Blacklist (MAC) values (@macAdress)";
                conn.Execute(query, obj);
            }
            catch (Exception e)
            {
                throw new Exception($"Error with inserting description into database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * Deletes the first matching MAC address from the blacklist.
         * @param MAC The MAC address to delete.
         */
        public void DeleteMAC(string MAC)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            try
            {
                conn.Open();
                string query = "DELETE TOP (1) from Blacklist where MAC = @MAC";
                conn.Execute(query, new
                {
                    MAC = MAC
                });
            }
            catch (Exception e)
            {
                throw new Exception($"Error with deleting hostname in database, error was {e.Message}", e);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
