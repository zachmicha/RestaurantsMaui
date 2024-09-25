using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{
    public static class Basket
    {
        public static List<Foods> BasketContent { get; set; } = new List<Foods>();
    }
}
