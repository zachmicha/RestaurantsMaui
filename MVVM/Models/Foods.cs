using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{


    public class Foods
    {
        public DateTime createdAt { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string picture { get; set; }
        public string id { get; set; }
        public string RestaurantId { get; set; }
    }

}
