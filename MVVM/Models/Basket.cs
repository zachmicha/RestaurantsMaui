using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{
    public class Basket
    {
        public static List<Basket> BasketContent { get; set; } = new List<Basket>();
        public Foods FoodItem { get; set; }
        public int NumberOfFoods { get; set; }
        public Basket()
        {
                
        }
    }
}
