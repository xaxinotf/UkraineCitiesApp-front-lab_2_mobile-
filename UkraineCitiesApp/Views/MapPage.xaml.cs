using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.ApplicationModel;
using UkraineCitiesApp.Models;
using Microsoft.Maui.Maps;
using System.Net.Http.Json;
using System.Text.Json;

namespace UkraineCitiesApp
{
    public partial class MapPage : ContentPage
    {
        private City _city;

        // ������ �� �������� ����� �������� API ������ Google Maps
        private const string GoogleMapsApiKey = "*********";

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
                //////var location = await Geolocation.GetLastKnownLocationAsync();
                /////if (location == null)
                /////{
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                ////}

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
                var startPin = new Pin
                {
                    Label = "�� ���",
                    Location = new Location(location.Latitude, location.Longitude)
                };

                var endPin = new Pin
                {
                    Label = _city.Name,
                    Location = cityPosition
                };

                map.Pins.Add(startPin);
                map.Pins.Add(endPin);

                // ������������ ������ ����������� �����
                var centerLocation = new Location(
                    (location.Latitude + cityPosition.Latitude) / 2,
                    (location.Longitude + cityPosition.Longitude) / 2);

                var distance = Distance.BetweenPositions(
                    new Location(location.Latitude, location.Longitude),
                    cityPosition);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(centerLocation, distance));

                // ������������ ��������� ��������
                var routeCoordinates = await GetRouteCoordinatesAsync(location, cityPosition);
                if (routeCoordinates != null)
                {
                    DrawRoute(routeCoordinates);
                }
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

        private async Task<List<Location>> GetRouteCoordinatesAsync(Location start, Location end)
        {
            try
            {
                using var client = new HttpClient();
                string requestUri = $"https://maps.googleapis.com/maps/api/directions/json?" +
                                    $"origin={start.Latitude},{start.Longitude}&destination={end.Latitude},{end.Longitude}" +
                                    $"&key={GoogleMapsApiKey}";

                var response = await client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
                    var routeCoordinates = new List<Location>();

                    // ������� ��������� �������� � ���� polyline
                    var steps = jsonResponse.GetProperty("routes")[0].GetProperty("legs")[0].GetProperty("steps");
                    foreach (var step in steps.EnumerateArray())
                    {
                        var polyline = step.GetProperty("polyline").GetProperty("points").GetString();
                        var decodedPoints = DecodePolyline(polyline);

                        routeCoordinates.AddRange(decodedPoints);
                    }

                    return routeCoordinates;
                }
                else
                {
                    throw new Exception("�� ������� �������� ������� � Google Maps.");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("������� ������������ ��������", ex.Message, "OK");
                return null;
            }
        }

        private List<Location> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
            {
                return null;
            }

            var poly = new List<Location>();
            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;

            while (index < polylineChars.Length)
            {
                // Calculate next latitude
                int sum = 0;
                int shifter = 0;
                int next5Bits;
                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // Calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5Bits = polylineChars[index++] - 63;
                    sum |= (next5Bits & 31) << shifter;
                    shifter += 5;
                } while (next5Bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5Bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                var point = new Location
                (
                    Convert.ToDouble(currentLat) / 1E5,
                    Convert.ToDouble(currentLng) / 1E5
                );
                poly.Add(point);
            }

            return poly;
        }

        private void DrawRoute(List<Location> routeCoordinates)
        {
            // ��������� �� �������� �� ����
            var polyline = new Polyline
            {
                StrokeColor = Colors.Blue,
                StrokeWidth = 5
            };

            foreach (var position in routeCoordinates)
            {
                polyline.Geopath.Add(position);
            }

            // ������ ��� �� �����
            map.MapElements.Add(polyline);
        }
    }
}
