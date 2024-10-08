using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using Restaurants.MVVM.Models;
using Restaurants.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace Restaurants.MVVM.Views;


public partial class BasketView : ContentPage, INotifyPropertyChanged
{
    BasketViewModel basketViewModel;
    public BasketView()
	{
		InitializeComponent();
        basketViewModel = new BasketViewModel();
        BindingContext = basketViewModel;

        WeakReferenceMessenger.Default.Register<CheckoutMessage>(this, (recipient, message) =>
        {
            DisplayAlert("Order", message.Summary, "OK");
        });
    }
   
}