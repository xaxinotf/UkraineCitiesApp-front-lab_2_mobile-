namespace UkraineCitiesApp.Models
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Значення за замовчуванням
        public string Email { get; set; } = string.Empty; // Значення за замовчуванням
    }
}
