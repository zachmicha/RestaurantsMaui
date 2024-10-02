using Restaurants.MVVM.Models;
using Restaurants.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurants.MVVM.ViewModels
{
    public class MainPageViewModel
    {
        JsonSerializerOptions _serializerOptions;
        private static readonly HttpClient _httpClient = new HttpClient();
        string baseUrl = "https://66dac750f47a05d55be5f0d5.mockapi.io/restaurants/";
        public ObservableCollection<Restaurant> RestaurantsList { get; set; }
        public ICommand RetrieveAllRestaurantsCommand { get; }
        public MainPageViewModel()
        {
            RestaurantsList = new ObservableCollection<Restaurant>();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Allows case-insensitive matching of JSON properties
            };
            RetrieveAllRestaurantsCommand = new Command(async () => await RetrieveAllRestaurants());
        }
        private async Task RetrieveAllRestaurants()
        {
            string allRestaurantsUrl = $"{baseUrl}Restaurants";
            try
            {
                var response = await _httpClient.GetAsync(allRestaurantsUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Raw JSON response: {jsonResponse}");

                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        var data = await JsonSerializer.DeserializeAsync<List<Restaurants.MVVM.Models.Restaurant>>(
                            responseStream,
                            _serializerOptions
                        );

                        if (data != null && data.Count > 0)
                        {
                            // Clear the list first, then add the new items
                            RestaurantsList.Clear();
                            foreach (var restaurant in data)
                            {
                                // Ensure the avatar property is correctly set
                                if (!string.IsNullOrEmpty(restaurant.avatar))
                                {
                                    Debug.WriteLine($"Restaurant: {restaurant.name}, Avatar URL: {restaurant.avatar}");
                                }
                                else
                                {
                                    Debug.WriteLine($"Warning: Restaurant {restaurant.name} has no avatar!");
                                }
                                RestaurantsList.Add(restaurant);
                                Debug.WriteLine($"Added restaurant: {restaurant.name}");  // Debugging line
                            }

                            Debug.WriteLine($"Deserialized {data.Count} restaurants.");
                            Debug.WriteLine($"Deserialized list of restaurants {RestaurantsList.Count} restaurants.");
                        }
                        else
                        {
                            Debug.WriteLine("No restaurants found in the response.");
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

        public async Task OnRestaurantSelected(Restaurant selectedRestaurant, INavigation navigation)
        {
            if (selectedRestaurant != null)
            {
                Debug.WriteLine($"{selectedRestaurant.name} and {selectedRestaurant.Location}");
                await navigation.PushAsync(new RestaurantDetailsView(selectedRestaurant));
            }
        }
    }
}
