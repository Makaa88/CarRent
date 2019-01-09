using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Npgsql;
using System.Windows.Controls;

namespace CarRent.Models
{
    public class DatabaseConnection
    {
        //For pascal ssh connection
        private SshClient client;
        //Forwared port to access database like it was in local
        private ForwardedPortLocal port;
        //Connection to database
        private NpgsqlConnection conn;
        //private static int i = 8001;

        //assign all connecations with username and password
        //TODO hide connection parameters to different file
        public DatabaseConnection()
        {
            client = new SshClient("pascal.fis.agh.edu.pl", "6libirt", "tingliessick");
            port = new ForwardedPortLocal("localhost", 8001, "localhost", 5432);
            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4}; CommandTimeout=1;",
                  "localhost", 8001, "u6libirt", "6libirt", "u6libirt");
            conn = new NpgsqlConnection(connectionString);
        }

        //Open connection
        public void OpenDataBase()
        {
            try
            {
                client.Connect();
                client.AddForwardedPort(port);
                port.Start();
                conn.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Błąd podczas otwarcia " + ex);
            }
        }

        //Close connection
        public void Close()
        {
            if (client.IsConnected)
            {
                conn.Close();
                port.Stop();
                client.RemoveForwardedPort(port);
                client.Disconnect();
            }
        }

        //FUnction to login
        public int LogIn(string username, string password)
        {
            if (client.IsConnected)
            {
                string prepareStatement = "SELECT id_klient FROM projekt.klient WHERE email=\'" + username + "\' AND haslo =\'" + password + "\'";
                using (var cmd = new NpgsqlCommand(prepareStatement, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // return reader.GetInt32(0);
                            while (reader.Read())
                                return reader.GetInt32(0);
                            return 0;
                        }
                        else
                            return 0;
                    }
                }
            }
            else return -1;
        }

        //Returns name of the user
        public string GetUsername(int id)
        {
            string prepareStatement = "Select imie FROM projekt.klient WHERE id_klient=" + id;
            using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                        return reader.GetString(0);
                }
            }

            return "ERROR";
        }

        //Launch an PGSQL function to change password
        public string ChangePassword(int id, string oldPass, string fNewPass, string sNewPass)
        {
            string prepareStatement = "Select * FROM zmien_haslo(" + id + ",\'"+ oldPass +"\',\'"+fNewPass+"\',\'"+sNewPass+"\')";
            using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.zmien_haslo", conn))
            {
                
              
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("id", NpgsqlTypes.NpgsqlDbType.Integer, id);
                cmd.Parameters.AddWithValue("stare", NpgsqlTypes.NpgsqlDbType.Text, oldPass);
                cmd.Parameters.AddWithValue("p_nowe", NpgsqlTypes.NpgsqlDbType.Text, fNewPass);
                cmd.Parameters.AddWithValue("d_nowe", NpgsqlTypes.NpgsqlDbType.Text, sNewPass);
                 using (NpgsqlDataReader reader = cmd.ExecuteReader())
                 {
                     while (reader.Read())
                         return reader.GetString(0);
                 }
              
            }
            return "ERROR";
        }

        public List<Car> GetAllCars(string type)
        {
            List<Car> list = new List<Car>();
            string prepareStatement = "Select id_pojazd, nazwa, ilosc_miejsc, koszt FROM projekt.pojazd WHERE typ=\'"+type+"\' AND dostepnosc = true";
            using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Car(reader.GetString(1), reader.GetInt32(2), reader.GetDecimal(3), reader.GetInt32(0)));
                        Console.WriteLine("Added");
                    }
                }
            }
            Console.WriteLine("db " + list.Count);
            return list;
        }

    }
}
