using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRent.Models;

namespace CarRent.ViewModels
{
    public class MainViewModel
    {
        private User user;
        private DatabaseConnection db;
        public MainViewModel(ref DatabaseConnection db, int id)
        {
            string name = db.GetUsername(id);
            user = new User(id, name);
            this.db = db;
        }

        public void CloseConnection()
        {
            db.Close();
        }

        public string GetName()
        {
            return user.Name;
        }

        public int GetID()
        {
            return user.ID;
        }

        public string GetUpcomingOrder()
        {
            return db.GetUpcomingOrder(user.ID);
        }
    }
}
