using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;
using System.Xml.Linq;
using Restaurants.MVVM.Models;
namespace Restaurants.MVVM.Views;

public partial class MainPage : ContentPage
{

    JsonSerializerOptions _serializerOptions;
    private static readonly HttpClient _httpClient = new HttpClient();
    string baseUrl = "https://66dac750f47a05d55be5f0d5.mockapi.io/restaurants/";
    public ObservableCollection<Restaurant> RestaurantsList { get; set; }
    public MainPage()
    {
        InitializeComponent();
        RestaurantsList = new ObservableCollection<Restaurant>();

        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Allows case-insensitive matching of JSON properties
        };
        BindingContext = this;
    }

    public ICommand RetrieveAllRestaurants => new Command(async () =>
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
     
    });

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        // Get the selected restaurant (or null if nothing selected)
        var selectedRestaurant = e.CurrentSelection.FirstOrDefault() as Restaurant;

        if (selectedRestaurant != null)
        {
            Debug.WriteLine($"{selectedRestaurant.name} and {selectedRestaurant.Location}");
            // Navigate to RestaurantDetailsView and pass the selected restaurant as a parameter
            await Navigation.PushAsync(new RestaurantDetailsView(selectedRestaurant));
          
            // Clear the selection to avoid re-triggering the event
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}

