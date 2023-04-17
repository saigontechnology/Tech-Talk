using MAUIAppDemo.ViewModels;

namespace MAUIAppDemo.Pages;

public partial class LoginChatPage : ContentPage
{
	public LoginChatPage()
	{
		InitializeComponent();
		BindingContext = new LoginChatViewModel();
	}
}