using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WEXO_Projekt_DAL.Model;


namespace WEXO_Projekt_DAL.DB
{
    /**
     * This class provides database operations for managing blacklisted MAC addresses, hostnames, and device descriptions.
     * Implements the IBlackListDB interface.
     */
    public class BlackListDB : IBlackListDB
    {
        private readonly string _connectionString;

        public BlackListDB(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DBConnectionString");
        }

        /**
         * Retrieves all blacklisted MAC addresses from the database.
         * @return List of blacklisted MAC address records.
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
         * @return List of blacklisted hostnames.
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
         * @return List of blacklisted device descriptions.
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
         * Adds a new device description to the blacklist. 
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
         * Deletes a specific device description from the blacklist.
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
         * Deletes a specific hostname from the blacklist.
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
         * Adds a new hostname to the blacklist.
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
         * Adds a new MAC address to the blacklist.
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
         * Deletes a specific MAC address from the blacklist.
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
