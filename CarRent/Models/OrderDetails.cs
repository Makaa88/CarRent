using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class OrderDetails
    {
        public int OrderID { get; private set; }
        public string Vehicle { get; private set; }
        public string StartTime { get; private set; }
        public string EndTime { get; private set; }
        public string Place { get; private set; }
        public decimal Cost { get; private set; }

       public  OrderDetails(int id, string v, string stime, string etime, string place, decimal cost)
        {
            this.OrderID = id;
            this.Vehicle = v;
            this.StartTime = stime;
            this.EndTime = etime;
            this.Place = place;
            this.Cost = cost;
        }
    }
}
