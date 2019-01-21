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
        private int orderNumber;

        public TravelViewModel(DatabaseConnection db, int id)
        {
            this.db = db;
            userID = id;
            this.orderNumber = 0;
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
            int firstPlace = db.AddNewPlace(StartCountryText, StartTownText, StartStreetText);
            if (firstPlace == -1) return -1;
            int secondPlace = db.AddNewPlace(EndCountryText, EndTownText, EndStreetText);
            if (secondPlace == -1) return -1;
            int travelTime = db.AddTravelTime(StartDate, StartHour, EndDate, EndHour);
            if (travelTime <= 0) return travelTime;
            int placeTime = db.AddPlaceTravelDependencies(firstPlace, secondPlace, travelTime);
            if (placeTime == -3) return -3;
            decimal totalCost = CalculateCost(id,StartDate, StartHour, EndDate, EndHour);
            if((int)totalCost != -4)
            {
                Console.WriteLine(totalCost);
                int orderNumber = db.AddOrder(userID, id, placeTime, totalCost);
                if (orderNumber != -5)
                    return 1;
                return -5;
            }
            else
            {
                return -4;
            }

        }

       

        private decimal CalculateCost(int id, string startDate, string startHour,string endDate, string endHour)
        {
            string[] sHour = startHour.Split(':');
            int hourStart = int.Parse(sHour[0]);
            sHour = endHour.Split(':');
            int hourEnd = int.Parse(sHour[0]);
            Console.WriteLine("Hours "+hourStart +" " + hourEnd);
            return db.CalculateCost(id,startDate,hourStart,endDate,hourEnd);
        }
    }
}
