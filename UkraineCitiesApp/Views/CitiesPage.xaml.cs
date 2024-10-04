using System.Collections.Generic;
using UkraineCitiesApp.Models;

namespace UkraineCitiesApp
{
    public partial class CitiesPage : ContentPage
    {
        public List<City> Cities { get; set; } = new List<City>(); // Ініціалізація властивості

        public CitiesPage(List<City> cities)
        {
            InitializeComponent();
            if (cities == null || cities.Count == 0)
            {
                DisplayAlert("Error", "No cities to display", "OK");
                return;
            }

            Cities = cities;
            BindingContext = this;
        }

        private async void ShowOnMap_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var city = button?.BindingContext as City; // Захист від null

            if (city != null)
            {
                await Navigation.PushAsync(new MapPage(city));
            }
        }
    }
}
