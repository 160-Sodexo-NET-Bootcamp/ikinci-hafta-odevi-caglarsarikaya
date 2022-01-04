using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle>> GetAll();
        Task Insert(Vehicle vehicle);
        void Delete(int id);
        void Update(Vehicle vehicle);
    }
}
