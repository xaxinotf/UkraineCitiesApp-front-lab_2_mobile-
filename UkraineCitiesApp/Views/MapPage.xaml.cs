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
                // ����� �� ������� ��� ����������
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("�����", "��������� ������ ������ �� ����������.", "OK");
                    return;
                }

                // ��������� ������� ����������
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
                    await DisplayAlert("�������", "�� ������� �������� ������� ����������.", "OK");
                    return;
                }

                // ��������� ��������� ���� ����� ������������
                var cityPosition = await GetCityPositionAsync(_city.Name);

                if (cityPosition == null)
                {
                    await DisplayAlert("�������", "�� ������� �������� ���������� ����.", "OK");
                    return;
                }

                // ��������� ������� �� �����
                map.Pins.Add(new Pin
                {
                    Label = "�� ���",
                    Location = new Location(location.Latitude, location.Longitude)
                });

                map.Pins.Add(new Pin
                {
                    Label = _city.Name,
                    Location = cityPosition
                });

                // ������������ ������ ����������� �����
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
                await DisplayAlert("�������", ex.Message, "OK");
            }
        }

        private async Task<Location> GetCityPositionAsync(string cityName)
        {
            // ������������ ������������ ��� ��������� ��������� ����
            var locations = await Geocoding.GetLocationsAsync($"{cityName}, ������");
            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                return new Location(location.Latitude, location.Longitude);
            }
            else
            {
                throw new Exception("�� ������� �������� ���������� ����.");
            }
        }
    }
}
