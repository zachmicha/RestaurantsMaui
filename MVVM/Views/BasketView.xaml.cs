using CommunityToolkit.Maui.Alerts;
using Restaurants.MVVM.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace Restaurants.MVVM.Views;


public partial class BasketView : ContentPage, INotifyPropertyChanged
{
	public ICommand CheckoutCommand { get; }
    public ICommand DeleteFromBasketCommand { get; }
    private async void DeleteFromBasket(Foods food)
    {
       
            Debug.WriteLine($"Deleting {food.name} to the basket");
          StaticResources.BasketItems.Remove(food);
        var toast = Toast.Make("Deleted item from the basket");
        await toast.Show();
    }
    public ObservableCollection<Foods> BasketItems => StaticResources.BasketItems;
    public BasketView()
	{
		InitializeComponent();
       
        StaticResources.BasketItems.CollectionChanged += (s, e) => MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(TotalPrice)));
        CheckoutCommand = new Command(DisplayCheckoutSummary);
        DeleteFromBasketCommand = new Command<Foods>(DeleteFromBasket);
        BindingContext = this;
    }


    public string TotalPrice
    {

        get
        {
            try
            {
                if (BasketItems == null || !BasketItems.Any())
                {
                    return "0"; // Return 0 if BasketItems is empty or null
                }
                else
                {
                decimal total= BasketItems.Sum(item => decimal.Parse(item.price, CultureInfo.InvariantCulture));
                return total.ToString("C", CultureInfo.CurrentCulture); //

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in TotalPrice calculation: " + ex.Message);
                return "0";
            }
        }
    }
    private void OnBasketItemsChanged()
    {

        MainThread.BeginInvokeOnMainThread(() =>
        {
            OnPropertyChanged(nameof(TotalPrice));
        });

    }
    async void DisplayCheckoutSummary()
    {
        string message = "";
        if (StaticResources.BasketItems.Any())
        {
            message= $"Your order has been successfully placed.\n Paid: {TotalPrice}";
        }
        else
        {
            message = "You need to add items to the basket to make an order";
        }
        await DisplayAlert("Order", message, "OK");
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}