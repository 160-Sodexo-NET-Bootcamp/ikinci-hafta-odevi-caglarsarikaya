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
    public class ContainerService : IContainerService
    {
        private readonly IRepository<Container> _repository;
        private readonly IUnitOfWork _uow;

        public ContainerService(IRepository<Container> repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }
        public void Delete(int id)
        {
            _repository.RemoveById(id);
            _uow.SaveChanges();
        }

        public async Task<IEnumerable<Container>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        public async Task Insert(Container container)
        {
            await _repository.AddAsync(container);
            await _uow.SaveChangesAsync();    
        }

        public void Update(Container container)
        {
            _repository.Update(container);
            _uow.SaveChanges(); 
        }
    }
}
