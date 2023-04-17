using MAUIAppDemo.ViewModels;

namespace MAUIAppDemo.Pages;

public partial class DogBreedsPage : ContentPage
{
	public DogBreedsPage()
	{
		InitializeComponent();
        // BindingContext = new DogBreedsViewModel();
        BindingContext = new DogBreeds2ViewModel();
	}
}