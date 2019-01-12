using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRent.Models;

namespace CarRent.ViewModels
{
    class OrderViewModel
    {
        private DatabaseConnection db;
        private int userID;
        private List<OrderDetails> orderList;
        
        public OrderViewModel(DatabaseConnection db, int id)
        {
            this.db = db;
            this.userID = id;
        }

        public List<OrderDetails> GetOrders(int type) => db.GetOrderDetails(userID,type);
    }
}
