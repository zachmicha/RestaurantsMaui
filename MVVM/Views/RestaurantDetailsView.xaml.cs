using Restaurants.MVVM.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;

namespace Restaurants.MVVM.Views;

public partial class RestaurantDetailsView : ContentPage
{
    private static readonly HttpClient _httpClient = new HttpClient();
    JsonSerializerOptions _serializerOptions;
    string baseUrl = "https://66dac750f47a05d55be5f0d5.mockapi.io/restaurants/";
    public Restaurant passedRestaurant{ get; set; }
    /*
        public DateTime createdAt { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
        public string Location { get; set; }
        public string Reviews { get; set; }
        public string id { get; set; }
    */
   static public ObservableCollection<Foods> Foods { get; private set; } = new ObservableCollection<Foods>();
    public ICommand RetrieveFoods { get; private set; }

    public RestaurantDetailsView()
	{
		InitializeComponent();
        passedRestaurant = new Restaurant();
        Foods = new ();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Allows case-insensitive matching of JSON properties
        };
        RetrieveFoods = new Command(async () => await LoadFoods());
        BindingContext = this;
    }
    public RestaurantDetailsView(Restaurant restaurant)
    {
        InitializeComponent();
        passedRestaurant = restaurant;
        Foods = new ();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Allows case-insensitive matching of JSON properties
        };
        RetrieveFoods = new Command(async () => await LoadFoods());
        BindingContext = this;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RestaurantDetailsView viewModel && viewModel.RetrieveFoods.CanExecute(null))
        {
            viewModel.RetrieveFoods.Execute(null); // Invoke the command
        }
    }
    private async Task LoadFoods()
    {
        string allFoods = $"{baseUrl}Foods/";
        try
        {
            var response = await _httpClient.GetAsync(allFoods);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Raw JSON response: {jsonResponse}");

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<Foods>>(responseStream, _serializerOptions);

                    if (data != null && data.Count > 0)
                    {
                        Foods.Clear();
                        foreach (var food in data)
                        {
                            Foods.Add(food);
                        }

                        Debug.WriteLine($"Deserialized {data.Count} foods.");
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
}
