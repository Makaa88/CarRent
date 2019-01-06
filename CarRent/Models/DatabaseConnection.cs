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
        public DatabaseConnection(/*string username, string password,ref Label testLabel*/)
        {
            /*SshClient*/ client = new SshClient("pascal.fis.agh.edu.pl", "6libirt", "tingliessick");
            //client.Connect();
            /*ForwardedPortLocal*/ port = new ForwardedPortLocal("localhost", 8001, "localhost", 5432);

           // port.Start();

            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4}; CommandTimeout=1;",
                  "localhost", 8001, "u6libirt", "6libirt", "u6libirt");

            /*NpgsqlConnection*/ conn = new NpgsqlConnection(connectionString);
          /*  Console.WriteLine(conn.CommandTimeout);
            Console.WriteLine(conn.ConnectionTimeout);*/
            //conn.Open();
            

           /* try
            {
              
                string prepareStatement = "SELECT id_osoba FROM test.osoba WHERE imie=\'" + username + "\' AND drugie_imie =\'" + password + "\'";
                var cmd = new NpgsqlCommand(prepareStatement, conn);
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                    testLabel.Content = "Zalogowano";
                else
                    testLabel.Content = "nie udalo sie";


            }
            catch(Exception e)
            {
                //testLabel.Content = "Wyjatek";
                Console.WriteLine(e);
            }
            finally
            {
                conn.Close();
                port.Stop();
                client.RemoveForwardedPort(port);
                port.Dispose();
                client.Disconnect();
            }*/

        }

        public void OpenDataBase()
        {
            try
            {
                client.Connect();
                client.AddForwardedPort(port);
                //if(!port.IsStarted)
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
               // port.Dispose();
                client.RemoveForwardedPort(port);
                client.Disconnect();
            }
        }


        public bool LogIn(string username, string password)
        {
            string prepareStatement = "SELECT id_osoba FROM test.osoba WHERE imie=\'" + username + "\' AND drugie_imie =\'" + password + "\'";
            using (var cmd = new NpgsqlCommand(prepareStatement, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                        return true;
                    else
                        return false;
                }
            }
        }

    }
}
