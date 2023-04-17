using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppShellDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MauiAppShellDemo.ViewModels
{
    public partial class DogBreeds2ViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Dog> _dogBreeds;

        public DogBreeds2ViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("dog_breeds.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var temp = JsonSerializer.Deserialize<List<Dog>>(contents);
            temp.AddRange(temp);
            DogBreeds = new ObservableCollection<Dog>(temp);
        }

        [RelayCommand]
        public async void ShowDetail(int id)
        {
            var dogbreed= DogBreeds.Where(d=>d.Id==id).FirstOrDefault();
            await Toast.Make($"{dogbreed.Name} was click!").Show();
        }
    }
}
