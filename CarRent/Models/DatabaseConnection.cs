using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Renci.SshNet;
using Npgsql;
using System.Windows.Controls;
using System.IO;

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

        //Add new conncections
        public DatabaseConnection()
        {
            var lines = File.ReadAllLines("connectiondata.dat");
            string user = lines[0];
            string password = lines[1];
            string dbpassword = lines[2];
            string dbname = lines[3];
            string dbuser = lines[4];

            client = new SshClient("pascal.fis.agh.edu.pl", user, password);
            port = new ForwardedPortLocal("localhost", 8001, "localhost", 5432);
            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                  "localhost", 8001, dbuser, dbpassword, dbname);
            conn = new NpgsqlConnection(connectionString);
        }

        //Open connection
        public int OpenDataBase()
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
                return -1;
            }

            return 1;
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
            try
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
            }
            catch(Exception)
            {
                Console.WriteLine("Blad poczas logowania");
                return -1;
            }

            return -1;
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

        public int AddNewPlace(string country, string town, string street)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_miejsce", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("kra", NpgsqlTypes.NpgsqlDbType.Text, country);
                cmd.Parameters.AddWithValue("miejscowos", NpgsqlTypes.NpgsqlDbType.Text, town);
                cmd.Parameters.AddWithValue("ulic", NpgsqlTypes.NpgsqlDbType.Text, street);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        return reader.GetInt32(0);
                }
            }
            return 0;
        }

        public int AddTravelTime(string StartDate, string StartHour, string EndDate, string EndHour)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_okres", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("sdata", NpgsqlTypes.NpgsqlDbType.Text, StartDate);
                cmd.Parameters.AddWithValue("sgodzina", NpgsqlTypes.NpgsqlDbType.Text, StartHour);
                cmd.Parameters.AddWithValue("kdata", NpgsqlTypes.NpgsqlDbType.Text, EndDate);
                cmd.Parameters.AddWithValue("kgodzina", NpgsqlTypes.NpgsqlDbType.Text, EndHour);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        return reader.GetInt32(0);
                }
            }
            return 0;
        }

        public int AddPlaceTravelDependencies(int place1, int place2, int time)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_miejsce_okres", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("miejsce1", NpgsqlTypes.NpgsqlDbType.Integer, place1);
                cmd.Parameters.AddWithValue("miejsce2", NpgsqlTypes.NpgsqlDbType.Integer, place2);
                cmd.Parameters.AddWithValue("czas", NpgsqlTypes.NpgsqlDbType.Integer, time);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        return reader.GetInt32(0);
                }
            }
            return 0;
        }

        public decimal CalculateCost(int id, string startDate, int hourStart, string endDate, int hourEnd)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.licz_koszt", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("idauta", NpgsqlTypes.NpgsqlDbType.Integer, id);
                cmd.Parameters.AddWithValue("sdata", NpgsqlTypes.NpgsqlDbType.Text, startDate);
                cmd.Parameters.AddWithValue("sgodzina", NpgsqlTypes.NpgsqlDbType.Integer, hourStart);
                cmd.Parameters.AddWithValue("kdata", NpgsqlTypes.NpgsqlDbType.Text, endDate);
                cmd.Parameters.AddWithValue("kgodzina", NpgsqlTypes.NpgsqlDbType.Integer, hourEnd);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        return reader.GetDecimal(0);
                }
            }
            return -1;
        }

        public int AddOrder(int idUser, int idCar, int idPlace, decimal cost)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_zamowienie", conn))
                {
                   /* string[] cena = cost.ToString().Split(',');
                    Console.WriteLine(cost + " " +cena[0] + " " + cena[1]);*/
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idklient", NpgsqlTypes.NpgsqlDbType.Integer, idUser);
                    cmd.Parameters.AddWithValue("idpojazd", NpgsqlTypes.NpgsqlDbType.Integer, idCar);
                    cmd.Parameters.AddWithValue("idmiejsce", NpgsqlTypes.NpgsqlDbType.Integer, idPlace);
                    cmd.Parameters.AddWithValue("cena", NpgsqlTypes.NpgsqlDbType.Numeric, cost);
                    //cmd.Parameters.AddWithValue("wartosc", NpgsqlTypes.NpgsqlDbType.Integer, int.Parse(cena[0]));
                    // cmd.Parameters.AddWithValue("reszta", NpgsqlTypes.NpgsqlDbType.Integer, int.Parse(cena[1]));
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            return reader.GetInt32(0);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("NIE WIDE FUNKCJI!!!");
                Console.WriteLine(e.ToString());
            }
            return -1;
        }
    }

  
}
