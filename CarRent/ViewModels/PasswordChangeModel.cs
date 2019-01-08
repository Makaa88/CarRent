using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRent.Models;

namespace CarRent.ViewModels
{
    public class PasswordChangeModel
    {
        private int userID;
        private DatabaseConnection db;

        public PasswordChangeModel(DatabaseConnection db, int id)
        {
            this.db = db;
            userID = id;
        }

        public string ChangePassword(string oldPass, string fNewPass, string sNewPass)
        {
           return db.ChangePassword(userID, oldPass, fNewPass, sNewPass);
        }
    }
}
