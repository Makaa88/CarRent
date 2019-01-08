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
        private SshClient client;
        private ForwardedPortLocal port;
        private NpgsqlConnection conn;
        private static int i = 8001;
        public DatabaseConnection(/*string username, string password,ref Label testLabel*/)
        {
            client = new SshClient("pascal.fis.agh.edu.pl", "6libirt", "tingliessick");
            port = new ForwardedPortLocal("localhost", 8001, "localhost", 5432);
            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4}; CommandTimeout=1;",
                  "localhost", 8001, "u6libirt", "6libirt", "u6libirt");
            conn = new NpgsqlConnection(connectionString);
        }

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


        public int LogIn(string username, string password)
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
              /*  string value = (string)cmd.ExecuteScalar();
                return value;*/
            }
            return "ERROR";
        }

    }
}
