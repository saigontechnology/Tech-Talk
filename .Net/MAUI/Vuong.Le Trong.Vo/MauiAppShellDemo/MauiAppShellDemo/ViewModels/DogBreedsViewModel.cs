using MauiAppShellDemo.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace MauiAppShellDemo.ViewModels
{
    public class DogBreedsViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private IList<Dog> _dogBreeds;

        public DogBreedsViewModel()
        {
            LoadData();
        }

        public IList<Dog> DogBreeds
        {
            get => _dogBreeds;
            set
            {
                _dogBreeds = value;
                OnPropertyChanged(); // reports this property
            }
        }
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        private async void LoadData()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("dog_breeds.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var temp = JsonSerializer.Deserialize<List<Dog>>(contents);
            DogBreeds = temp;
        }
    }
}
