using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{
    public class CheckoutMessage
    {
        public string Summary { get; }

        public CheckoutMessage(string summary)
        {
            Summary = summary;
        }
    }
}
