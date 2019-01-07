using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class User
    {
        int ID { get; set; }
        public string Name { get; set; }

        public User(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
