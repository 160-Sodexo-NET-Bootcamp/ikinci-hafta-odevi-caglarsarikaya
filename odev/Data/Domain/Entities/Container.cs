
namespace Data.Domain.Entities
{
    public class Container
    {
        public int Id { get; set; } 
        public double LocationX { get; set; } = 0;
        public double LocationY { get; set; } = 0;
        public Vehicle Vehicle { get; set; }
        public int? VehicleId { get; set; }
    }
}
