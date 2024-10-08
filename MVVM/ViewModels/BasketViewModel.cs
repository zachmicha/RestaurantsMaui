using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using Restaurants.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Restaurants.MVVM.ViewModels
{
   public class BasketViewModel : INotifyPropertyChanged
    {
        public ICommand CheckoutCommand { get; }
        //public void DisplayCheckoutSummary()
        //  {
        //      string message = "";
        //      if (StaticResources.BasketItems.Any())
        //      {
        //          message = $"Your order has been successfully placed.\n Paid: {TotalPrice}";
        //      }
        //      else
        //      {
        //          message = "You need to add items to the basket to make an order";
        //      }
        //      WeakReferenceMessenger.Default.Send(message);
        //  }
        private void ExecuteCheckout()
        {
            string message = "You need to add items to the basket to make an order";
            if (StaticResources.BasketItems.Any())
            {
                message = $"Your order has been successfully placed.\n Paid: {TotalPrice}";
            }

            // Send the message
            WeakReferenceMessenger.Default.Send(new CheckoutMessage(message));
        }
        public ICommand DeleteFromBasketCommand { get; }
        private async void DeleteFromBasket(Foods food)
        {

            Debug.WriteLine($"Deleting {food.name} to the basket");
            StaticResources.BasketItems.Remove(food);
            var toast = Toast.Make("Deleted item from the basket");
            await toast.Show();
        }
        public ObservableCollection<Foods> BasketItems => StaticResources.BasketItems;
        public BasketViewModel()
        {
            StaticResources.BasketItems.CollectionChanged += (s, e) => MainThread.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(TotalPrice)));
            CheckoutCommand = new Command(ExecuteCheckout);
            DeleteFromBasketCommand = new Command<Foods>(DeleteFromBasket);
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
                        decimal total = BasketItems.Sum(item => decimal.Parse(item.price, CultureInfo.InvariantCulture));
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
      
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
