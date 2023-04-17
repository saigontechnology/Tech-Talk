using MauiAppShellDemo.ViewModels;

namespace MauiAppShellDemo.Pages;

public partial class DogBreedsPage : ContentPage
{
	public DogBreedsPage()
	{
		InitializeComponent();
        // BindingContext = new DogBreedsViewModel();
        BindingContext = new DogBreeds2ViewModel();
	}
}