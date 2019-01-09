using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent.Models
{

    public class Car
    {
        public string ModelName { get; private set; }
        public int SeatsAmmount { get; private set; }
        public decimal Price { get; private set; }
        public int CarID { get; private set; }
       // public string Type { get; private set; }

        public Car(string model, int seats, decimal price, int id)
        {
            this.ModelName = model;
            this.SeatsAmmount = seats;
            this.Price = price;
            this.CarID = id;
           // this.Type = type;
        }

        public override string ToString()
        {
            return ModelName + " " + SeatsAmmount + " " + Price;
        }

    }
}
