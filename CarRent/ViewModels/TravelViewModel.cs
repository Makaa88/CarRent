using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Views;

namespace CarRent.ViewModels
{
    public class TravelViewModel
    {
        private DatabaseConnection db;
        private int userID;
        private List<Car> cars;

        public TravelViewModel(DatabaseConnection db, int id)
        {
            this.db = db;
            userID = id;
        }

        public List<Car> GetCoaches()
        {
            cars = db.GetAllCars("Autobus");
            return cars;
        }

        public List<Car> GetBuses()
        {
           
            cars = db.GetAllCars("Bus");
            return cars;
        }

        //TODO Change this method(to many params)
        public int ArrangeTravel(int id, string StartCountryText, string StartTownText, string StartStreetText, string StartDate, string StartHour, string EndCountryText, string EndTownText, string EndStreetText, string EndDate, string EndHour)
        {
           /* int firstPlace = db.AddNewPlace(StartCountryText, StartTownText, StartStreetText);
            int secondPlace = db.AddNewPlace(EndCountryText, EndTownText, EndStreetText);*/

            return 0;
        }
    }
}
