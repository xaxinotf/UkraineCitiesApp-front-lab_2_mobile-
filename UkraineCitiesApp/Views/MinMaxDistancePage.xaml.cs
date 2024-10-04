using UkraineCitiesApp.Models;

namespace UkraineCitiesApp.Views
{
    public partial class MinMaxDistancePage : ContentPage
    {
        public MinMaxDistancePage(MinMaxDistance minMaxDistance)
        {
            InitializeComponent();
            BindingContext = minMaxDistance;
        }
    }
}
