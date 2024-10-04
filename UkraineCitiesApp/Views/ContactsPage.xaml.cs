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

            // ������������ ��� ��������
            AllContacts = contacts;

            // Գ�������� ������� �������� (�, �� �� ����������� �� @ukr.net)
            FavoriteContacts = contacts.Where(c => !c.Email.EndsWith("@ukr.net")).ToList();

            // ����'���� ����� �� �������
            BindingContext = this;
        }
    }
}
