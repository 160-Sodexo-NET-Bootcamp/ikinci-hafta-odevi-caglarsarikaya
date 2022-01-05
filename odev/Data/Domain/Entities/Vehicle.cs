
using System.Collections.Generic;

namespace Data.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public double LocationX { get; set; } = 0;
        public double LocationY { get; set; } = 0;
        public ICollection<Container> Containers { get; set; }

    }
}
