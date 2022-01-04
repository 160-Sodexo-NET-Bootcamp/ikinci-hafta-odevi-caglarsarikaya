using Business.Abstract;
using Core.Abstract;
using Data.Domain.Entities;
using Data.IUnitOfWork;
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
        private readonly IUnitOfWork _uow;
        public VehicleService(IRepository<Vehicle> repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        public void Delete(int id)
        {
            _repository.RemoveById(id);
            _uow.SaveChanges();
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public async Task Insert(Vehicle vehicle)
        {
            await _repository.AddAsync(vehicle);
            await _uow.SaveChangesAsync();
        }

        public void Update(Vehicle vehicle)
        {
            _repository.Update(vehicle);
            _uow.SaveChanges();
        }
    }
}
