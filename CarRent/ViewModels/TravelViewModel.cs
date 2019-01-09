using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRent.Models;

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
    }
}
