using CommunityToolkit.Maui.Alerts;
using Restaurants.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurants.MVVM.ViewModels
{
    public class RestaurantDetailsViewModel : INotifyPropertyChanged
    {
       public bool _dataLoaded = false;
        private JsonSerializerOptions _serializerOptions;
        private void InitializeSerializerOptions()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        //Commands

        public ICommand LoadFoodsCommand { get;}
        public ICommand DeleteFromBasketCommand { get; }

        public ICommand AddToBasketCommand { get; }
        //Collections
        private ObservableCollection<Foods> _filteredFoods = new ObservableCollection<Foods>();
        public ObservableCollection<Foods> FilteredFoods
        {
            get => _filteredFoods;
            set
            {
                _filteredFoods = value;
                OnPropertyChanged(nameof(FilteredFoods));
            }
        }
        private Restaurant _passedRestaurant;
        public Restaurant PassedRestaurant
        {
            get => _passedRestaurant;
            set
            {
                if (_passedRestaurant != value)
                {
                    _passedRestaurant = value;
                    OnPropertyChanged(nameof(PassedRestaurant));
                    OnPropertyChanged(nameof(FilteredFoods)); // Notify that FilteredFoods has changed
                }
            }
        }
        private string baseUrl = "https://66dac750f47a05d55be5f0d5.mockapi.io/restaurants/";
        public RestaurantDetailsViewModel(Restaurant PassedRestaurant)
        {
            this.PassedRestaurant = PassedRestaurant;
            InitializeSerializerOptions();
            LoadFoodsCommand = new Command(async () => await LoadFoods());
            DeleteFromBasketCommand = new Command<Foods>(DeleteFromBasket);
            AddToBasketCommand = new Command<Foods>(AddToBasketMethod);
        }
        private void FilterFoods()
        {
            // Clear previous filtered foods
            FilteredFoods.Clear();

            // Filter foods based on the passed restaurant ID
            var filtered = StaticResources.AllFoods.Where(food => food.RestaurantId == PassedRestaurant.id); // Assuming Foods has a property named RestaurantId

            foreach (var food in filtered)
            {
                FilteredFoods.Add(food);
            }
        }

        private async void DeleteFromBasket(Foods food)
        {
            if (food != null)
            {
                Debug.WriteLine($"Deleting {food.name} from the basket");
                //Find the matching item by 'id'
                var itemToRemove = StaticResources.BasketItems.FirstOrDefault(f => f.id == food.id);

                if (itemToRemove != null)
                {
                    StaticResources.BasketItems.Remove(itemToRemove);
                    var toast = Toast.Make("Deleted item from the basket");
                    await toast.Show();
                }
                else
                {
                    Debug.WriteLine("Item not found in basket");
                }
            }
        }

        public async Task LoadFoods()
        {
            string allFoodsUrl = $"{baseUrl}Foods/";
            try
            {
                var response = await StaticResources._httpClient.GetAsync(allFoodsUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Raw JSON response: {jsonResponse}");

                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<List<Foods>>(responseStream, _serializerOptions);

                        if (data != null && data.Count > 0)
                        {
                            StaticResources.AllFoods.Clear();
                            foreach (var food in data)
                            {
                                StaticResources.AllFoods.Add(food);
                            }

                            Debug.WriteLine($"Deserialized {data.Count} foods.");
                            FilterFoods(); // Call to filter foods after loading
                            _dataLoaded = true;
                        }
                        else
                        {
                            Debug.WriteLine("No foods found in the response.");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
      async void AddToBasketMethod(Foods food)
        {
            Debug.WriteLine("Added Command activated");
            if (food !=null && !StaticResources.BasketItems.Contains(food))
            {
                Debug.WriteLine($"Adding {food.name} to the basket");
                StaticResources.BasketItems.Add(food);
                var toast = Toast.Make("Added item to the basket");
                await toast.Show();
            }
        }
    }
}
