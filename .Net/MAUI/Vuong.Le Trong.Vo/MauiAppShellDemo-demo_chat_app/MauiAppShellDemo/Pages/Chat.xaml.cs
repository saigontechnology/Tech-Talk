using MAUIAppDemo.ViewModels;

namespace MAUIAppDemo.Pages;

public partial class Chat : ContentPage
{
	private readonly ChatViewModel _viewModel;
	public Chat()
	{
		InitializeComponent();
		var vm = new ChatViewModel();

        BindingContext = vm;
		_viewModel = vm;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
		if(_viewModel.Messages.Count > 0)
		{
            messageCollectionView.ScrollTo(0, 1, ScrollToPosition.End, false);
        }
    }
}