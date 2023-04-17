using MauiAppShellDemo.ViewModels;

namespace MauiAppShellDemo.Pages;

public partial class Chat : ContentPage
{
	public Chat()
	{
		InitializeComponent();
		BindingContext = new ChatViewModel();
	}
}