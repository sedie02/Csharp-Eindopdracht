using System.Collections.Generic;

namespace Dierentuin.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Animal> Animals { get; set; } = new List<Animal>();
    }
}
