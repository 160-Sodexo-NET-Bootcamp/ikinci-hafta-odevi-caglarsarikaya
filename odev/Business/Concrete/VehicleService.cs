using Business.Abstract;
using Core.Abstract;
using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<Vehicle> _repository;

        public VehicleService(IRepository<Vehicle> repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _repository.GetAllAsync();
        }
    }
}
