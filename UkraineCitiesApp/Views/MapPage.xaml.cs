using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.ApplicationModel;
using UkraineCitiesApp.Models;
using Microsoft.Maui.Maps;

namespace UkraineCitiesApp
{
    public partial class MapPage : ContentPage
    {
        private City _city;

        public MapPage(City city)
        {
            InitializeComponent();
            _city = city;
            LoadRoute();
        }

        private async void LoadRoute()
        {
            try
            {
                // Запит на дозволи для геолокації
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Дозвіл", "Необхідно надати доступ до геолокації.", "OK");
                    return;
                }

                // Отримання поточної геолокації
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location == null)
                {
                    await DisplayAlert("Помилка", "Не вдалося отримати поточну геолокацію.", "OK");
                    return;
                }

                // Отримання координат міста через геокодування
                var cityPosition = await GetCityPositionAsync(_city.Name);

                if (cityPosition == null)
                {
                    await DisplayAlert("Помилка", "Не вдалося отримати координати міста.", "OK");
                    return;
                }

                // Додавання маркерів на карту
                map.Pins.Add(new Pin
                {
                    Label = "Ви тут",
                    Location = new Location(location.Latitude, location.Longitude)
                });

                map.Pins.Add(new Pin
                {
                    Label = _city.Name,
                    Location = cityPosition
                });

                // Встановлення області відображення карти
                var centerLocation = new Location(
                    (location.Latitude + cityPosition.Latitude) / 2,
                    (location.Longitude + cityPosition.Longitude) / 2);

                var distance = Distance.BetweenPositions(
                    new Location(location.Latitude, location.Longitude),
                    cityPosition);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(centerLocation, distance));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Помилка", ex.Message, "OK");
            }
        }

        private async Task<Location> GetCityPositionAsync(string cityName)
        {
            // Використання геокодування для отримання координат міста
            var locations = await Geocoding.GetLocationsAsync($"{cityName}, Україна");
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                return new Location(location.Latitude, location.Longitude);
            }
            else
            {
                throw new Exception("Не вдалося отримати координати міста.");
            }
        }
    }
}
