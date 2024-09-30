using Restaurants.MVVM.Models;

namespace Restaurants.MVVM.Views;

public partial class BasketView : ContentPage
{
	Basket myBasket;
	public BasketView()
	{
		InitializeComponent();
		myBasket = new Basket();
		myBasket.NumberOfFoods = 5;
		BindingContext = myBasket;
	}
}