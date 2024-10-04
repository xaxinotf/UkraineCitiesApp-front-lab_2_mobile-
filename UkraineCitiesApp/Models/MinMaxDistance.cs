using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkraineCitiesApp.Models
{
    public class MinMaxDistance
    {
        public City Max { get; set; } = new City(); // Ініціалізація об'єкта
        public City Min { get; set; } = new City(); // Ініціалізація об'єкта
    }
}
