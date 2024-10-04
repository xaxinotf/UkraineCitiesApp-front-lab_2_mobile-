using UkraineCitiesApp.Models;

namespace UkraineCitiesApp
{
    public partial class ContactsPage : ContentPage
    {
        public List<ContactModel> Contacts { get; set; }

        public ContactsPage(List<ContactModel> contacts)
        {
            InitializeComponent();
            Contacts = contacts;
            BindingContext = this;
        }
    }
}
