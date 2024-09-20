using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{


    //public class Rootobject
    //{
    //    public Restaurant[] Property1 { get; set; }
    //}

    public class Restaurant
    {
        public DateTime createdAt { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string Location { get; set; }
        public string Reviews { get; set; }
        public string id { get; set; }
    }



}
