using CommunityToolkit.Mvvm.ComponentModel;

namespace MAUIAppDemo.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        public bool _isBusy;
    }
}
