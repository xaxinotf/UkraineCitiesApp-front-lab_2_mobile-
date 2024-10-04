using UkraineCitiesApp.Models;

namespace UkraineCitiesApp
{
    public partial class ContactsPage : TabbedPage
    {
        public List<ContactModel> AllContacts { get; set; }
        public List<ContactModel> FavoriteContacts { get; set; }

        public ContactsPage(List<ContactModel> contacts)
        {
            InitializeComponent();

            // Встановлення усіх контактів
            AllContacts = contacts;

            // Фільтрація обраних контактів (ті, що не закінчуються на @ukr.net)
            FavoriteContacts = contacts.Where(c => !c.Email.EndsWith("@ukr.net")).ToList();

            // Прив'язка даних до вкладок
            BindingContext = this;
        }
    }
}
