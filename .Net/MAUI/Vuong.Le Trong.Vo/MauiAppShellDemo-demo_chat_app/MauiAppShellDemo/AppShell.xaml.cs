using MAUIAppDemo.Pages;

namespace MAUIAppDemo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        RegisterRoute();
    }
    private void RegisterRoute()
    {
        Routing.RegisterRoute(nameof(Chat), typeof(Chat));
    }
}
