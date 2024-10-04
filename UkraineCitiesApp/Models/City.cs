namespace UkraineCitiesApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Додаємо значення за замовчуванням, щоб уникнути null
        public double Distance { get; set; }
        public int Population { get; set; }
    }
}
