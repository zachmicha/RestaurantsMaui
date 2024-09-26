using Restaurants.MVVM.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;

namespace Restaurants.MVVM.Views;

public partial class RestaurantDetailsView : ContentPage, INotifyPropertyChanged
{
    bool _dataLoaded = false;
    private static readonly HttpClient _httpClient = new HttpClient();
    private JsonSerializerOptions _serializerOptions;
    private string baseUrl = "https://66dac750f47a05d55be5f0d5.mockapi.io/restaurants/";
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

    static public ObservableCollection<Foods> AllFoods { get; private set; } = new ObservableCollection<Foods>();
    public ObservableCollection<Foods> FilteredFoods { get; private set; } = new ObservableCollection<Foods>();

    public ICommand LoadFoodsCommand { get; private set; }

    public RestaurantDetailsView()
    {
        InitializeComponent();
        InitializeSerializerOptions();
        LoadFoodsCommand = new Command(async () => await LoadFoods());
        BindingContext = this;
    }
    public ICommand AddToBasketCommand => new Command(() =>
    {
        var u = 1;
    });
    public ICommand DeleteFromBasketCommand => new Command(() =>
    {
        var u = 1;
    });
    public RestaurantDetailsView(Restaurant restaurant) : this()
    {
        PassedRestaurant = restaurant;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (! _dataLoaded)
        {
            LoadFoodsCommand.Execute(null);
            _dataLoaded = true;
        }        
    }

    private void InitializeSerializerOptions()
    {
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private async Task LoadFoods()
    {
        string allFoodsUrl = $"{baseUrl}Foods/";
        try
        {
            var response = await _httpClient.GetAsync(allFoodsUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Raw JSON response: {jsonResponse}");

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<Foods>>(responseStream, _serializerOptions);

                    if (data != null && data.Count > 0)
                    {
                        AllFoods.Clear();
                        foreach (var food in data)
                        {
                            AllFoods.Add(food);
                        }

                        Debug.WriteLine($"Deserialized {data.Count} foods.");
                        FilterFoods(); // Call to filter foods after loading
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

    private void FilterFoods()
    {
        // Clear previous filtered foods
        FilteredFoods.Clear();

        // Filter foods based on the passed restaurant ID
        var filtered = AllFoods.Where(food => food.RestaurantId == PassedRestaurant.id); // Assuming Foods has a property named RestaurantId

        foreach (var food in filtered)
        {
            FilteredFoods.Add(food);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
