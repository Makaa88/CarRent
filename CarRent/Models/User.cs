using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class User
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public User(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
