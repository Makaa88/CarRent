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
        private DatabaseConnection db;
        public MainViewModel(ref DatabaseConnection db)
        {
            this.db = db;
        }

        internal void CloseConnection()
        {
            db.Close();
        }
    }
}
