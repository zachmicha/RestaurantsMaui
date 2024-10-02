using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;
using System.Xml.Linq;
using Restaurants.MVVM.Models;
using Restaurants.MVVM.ViewModels;
namespace Restaurants.MVVM.Views;

public partial class MainPage : ContentPage
{

  
    public MainPage()
    {
        InitializeComponent();
      
        BindingContext = new MainPageViewModel();
    }


  

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        
        // Get the selected restaurant (or null if nothing selected)
        var selectedRestaurant = e.CurrentSelection.FirstOrDefault() as Restaurant;

        if (selectedRestaurant != null)
        {
            var viewModel = BindingContext as MainPageViewModel;
            if (viewModel != null)
            {
                await viewModel.OnRestaurantSelected(selectedRestaurant, Navigation);
            }

                ((CollectionView)sender).SelectedItem = null;
        }
    }
}

