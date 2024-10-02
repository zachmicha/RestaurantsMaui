using Restaurants.MVVM.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using System.Linq;
using System.ComponentModel;
using Restaurants.MVVM.ViewModels;

namespace Restaurants.MVVM.Views;

public partial class RestaurantDetailsView : ContentPage, INotifyPropertyChanged
{
    #region VariablesAndProperties

    RestaurantDetailsViewModel viewModel;
    
  
    #endregion


    public RestaurantDetailsView(Restaurant restaurant)
    {
        InitializeComponent();
        viewModel = new RestaurantDetailsViewModel(restaurant);
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!viewModel._dataLoaded)
        {
            viewModel.LoadFoodsCommand.Execute(null);
        }
    }

}
