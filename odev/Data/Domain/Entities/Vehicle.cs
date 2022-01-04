
namespace Data.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public int LocationX { get; set; } = 0;
        public int LocationY { get; set; } = 0;

    }
}
