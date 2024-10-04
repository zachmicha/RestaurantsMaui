using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.MVVM.Models
{
    public static class StaticResources
    {
        public static readonly HttpClient _httpClient = new HttpClient();
        static public ObservableCollection<Foods> AllFoods { get; private set; } = new ObservableCollection<Foods>();
        static public ObservableCollection<Foods> BasketItems { get; private set; } = new ObservableCollection<Foods>();
    }
}
