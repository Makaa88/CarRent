using System;
using System.Collections.Generic;
using System.IO;
using Renci.SshNet;
using Npgsql;


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
           /* var lines = File.ReadAllLines("connectiondata.dat");
            string user = lines[0];
            string password = lines[1];
            string dbpassword = lines[2];
            string dbname = lines[3];
            string dbuser = lines[4];*/

            client = new SshClient("pascal.fis.agh.edu.pl", "6libirt", "secretPass");
            port = new ForwardedPortLocal("localhost", 8001, "localhost", 5432); 
            string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                  "localhost", 8001, "u6libirt", "6libirt", "u6libirt");
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
            try
            {
                string prepareStatement = "Select imie FROM projekt.klient WHERE id_klient=" + id;
                using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            return reader.GetString(0);
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Wyjatek w GetUsername");
                return "Błąd";
            }

            return "ERROR";
        }

        //Launch an PGSQL function to change password
        public string ChangePassword(int id, string oldPass, string fNewPass, string sNewPass)
        {
            try
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
            }
            catch(Exception)
            {
                Console.WriteLine("Wyjatek w ChangePassword");
                return "Wystąpił błąd";
            }
            return "ERROR";
        }

        public List<Car> GetAllCars(string type)
        {
            List<Car> list = new List<Car>();
            string prepareStatement = "Select id_pojazd, nazwa, ilosc_miejsc, koszt FROM projekt.pojazd WHERE typ=\'"+type+"\' AND dostepnosc = true";
            try
            {
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
            }
            catch(Exception)
            {
                Console.WriteLine("Wyjatek w GetAllCars");
                return null;
            }
            Console.WriteLine("db " + list.Count);
            return list;
        }

        public int AddNewPlace(string country, string town, string street)
        {
            try
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
            }
            catch(Exception)
            {
                Console.WriteLine("Wyjatek w AddNewPlace");
                return -1;
            }
            return -1;
        }

        public int AddTravelTime(string StartDate, string StartHour, string EndDate, string EndHour)
        {

            try
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
            }
            catch(Exception e)
            {
                Console.WriteLine("Wyjatek w AddTravelTime");
                Console.WriteLine(e.ToString());
                return -3;
            }
            return -3;
        }

        public int AddPlaceTravelDependencies(int place1, int place2, int time)
        {
            try
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
            }
            catch(Exception e)
            {
                Console.WriteLine("Wyjatek w AddPlaceTravelDependencies");
                Console.WriteLine(e.ToString());
                return -3;
            }
            return -3;
        }

        public decimal CalculateCost(int id, string startDate, int hourStart, string endDate, int hourEnd)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.licz_koszt", conn))
                {
                    Console.WriteLine(hourStart);
                    Console.WriteLine(hourEnd);
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
            }
            catch(Exception e)
            {
                Console.WriteLine("Wyjatek w CalculateCost");
                Console.WriteLine(e);
                return -4;
            }
            return -4;
        }

        public int AddOrder(int idUser, int idCar, int idPlace, decimal cost)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_zamowienie", conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idklient", NpgsqlTypes.NpgsqlDbType.Integer, idUser);
                    cmd.Parameters.AddWithValue("idpojazd", NpgsqlTypes.NpgsqlDbType.Integer, idCar);
                    cmd.Parameters.AddWithValue("idmiejsce", NpgsqlTypes.NpgsqlDbType.Integer, idPlace);
                    cmd.Parameters.AddWithValue("cena", NpgsqlTypes.NpgsqlDbType.Numeric, cost);
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
                return -5;
            }
            return -5;
        }

        public List<OrderDetails> GetOrderDetails(int id, int type)
        {
            List<OrderDetails> list = new List<OrderDetails>();
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string prepareStatement = "";
            if(type == 1)
                prepareStatement += "Select * FROM projekt.dane_zamowienia WHERE id_klient="+id+" AND data_koniec <=\'"+date + "\'";
            else if (type == 3)
                prepareStatement += "Select * FROM projekt.dane_zamowienia WHERE id_klient=" + id + " AND data_koniec >=\'" + date + "\'";
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
                {
             
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idorder = reader.GetInt32(1);
                            string vehicle = reader.GetString(2);
                            string stime = reader.GetDate(3).ToString() + " " + reader.GetTimeSpan(4);
                            string etime = reader.GetDate(5).ToString() + " " + reader.GetTimeSpan(6);
                            decimal cost = reader.GetDecimal(7);
                            string place = reader.GetString(8) + " - " + reader.GetString(9);
                            list.Add(new OrderDetails(idorder, vehicle, stime, etime, place, cost));
                            Console.WriteLine("orderadded");
                        }                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("NIE WIDE FUNKCJI!!!");
                Console.WriteLine(e.ToString());
            }
            return list;
        }

        public string AddNewUser(string name, string surname, string address, string phone, string mail, string pass1, string pass2)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.dodaj_klienta", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("im", NpgsqlTypes.NpgsqlDbType.Varchar, name);
                    cmd.Parameters.AddWithValue("naz", NpgsqlTypes.NpgsqlDbType.Varchar, surname);
                    cmd.Parameters.AddWithValue("adr", NpgsqlTypes.NpgsqlDbType.Varchar,address );
                    cmd.Parameters.AddWithValue("tel", NpgsqlTypes.NpgsqlDbType.Bigint, int.Parse(phone));
                    cmd.Parameters.AddWithValue("mail", NpgsqlTypes.NpgsqlDbType.Varchar, mail);
                    cmd.Parameters.AddWithValue("has1", NpgsqlTypes.NpgsqlDbType.Varchar, pass1);
                    cmd.Parameters.AddWithValue("has2", NpgsqlTypes.NpgsqlDbType.Varchar, pass2);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            return reader.GetString(0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Wystąpił błąd";
            }
            return "Nieoczekiwany return";
        }


        public int DeleteOrder(int idOrder)
        {
            try
            {
                string prepareStatement = "DELETE FROM projekt.zamowienie WHERE id_zamowienia=" + idOrder;
                using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
                {
                    
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
           // return -1;
        }

        public string GetUpcomingOrder(int userID)
        {
            try
            {

                string date = DateTime.Today.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("HH:mm:ss");
                Console.WriteLine(date);
                Console.WriteLine(time);
                string prepareStatement = "SELECT data_start, godzina_start, data_koniec, godzina_koniec, koszt, miejscowosc, kon " +
                    "FROM projekt.dane_zamowienia WHERE id_klient=" + userID + " AND (data_start >\'" + date + "\' OR (data_start=\'"+date+"\' AND godzina_start >\'" + time + "\') ) ORDER BY data_start ASC LIMIT 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(prepareStatement, conn))
                {

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string travelDate = reader.GetDate(0).ToString()+" - " + reader.GetDate(2).ToString();
                                string travelTime = reader.GetTimeSpan(1).ToString();
                                string travelPlace = reader.GetString(5) + " - " + reader.GetString(6);
                                string cost = reader.GetDecimal(4).ToString();
                                return travelPlace + " w dniach: " + travelDate + "\nrozpocznie się o godzinie " + travelTime + "\nkoszt " + cost+"zł";
                            }
                        }
                        else
                            return "Brak nadchodzących podróży";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "Błąd podczas połączenia";
            }
            return "Błąd poczdasz połączenia";
            
        }

        public void Synchronize()
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("projekt.synch", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
   
        }
    }


  

}
