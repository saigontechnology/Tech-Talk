using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIAppDemo.Pages;

namespace MAUIAppDemo.ViewModels
{
    public partial class LoginChatViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string _username;

        [RelayCommand]
        public async void Login()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync($"{nameof(Chat)}",
                       new Dictionary<string, object> { { "username", Username }
                       });
                });
            }
            else
            {
                await Toast.Make("Please insert username").Show();
            }
        }
    }
}
