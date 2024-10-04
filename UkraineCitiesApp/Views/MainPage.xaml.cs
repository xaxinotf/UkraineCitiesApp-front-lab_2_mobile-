using UkraineCitiesApp.Models;
using UkraineCitiesApp.Services;
using UkraineCitiesApp.Views;

namespace UkraineCitiesApp;

public partial class MainPage : ContentPage
{
    private readonly ApiService _apiService;

    public MainPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }

    private async void ShowAllCities_Clicked(object sender, EventArgs e)
    {
        try
        {
            var cities = await _apiService.GetCitiesAsync();
            await Navigation.PushAsync(new CitiesPage(cities));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void ShowSampleCities_Clicked(object sender, EventArgs e)
    {
        try
        {
            var cities = await _apiService.GetSampleCitiesAsync();
            await Navigation.PushAsync(new CitiesPage(cities));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void ShowMinMaxDistance_Clicked(object sender, EventArgs e)
    {
        try
        {
            var minMax = await _apiService.GetMinMaxDistanceAsync();
            await Navigation.PushAsync(new MinMaxDistancePage(minMax));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void ShowContacts_Clicked(object sender, EventArgs e)
    {
        try
        {
            var contacts = await _apiService.GetContactsAsync();
            await Navigation.PushAsync(new ContactsPage(contacts));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void ShowDeveloperInfo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeveloperInfoPage());
    }
}
