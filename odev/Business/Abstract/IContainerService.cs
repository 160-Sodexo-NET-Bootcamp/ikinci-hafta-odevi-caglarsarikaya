using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IContainerService
    {
        Task<IEnumerable<Container>> GetAll();
        Task Insert(Container container);
        void Delete(int id);
        void Update(Container container);
    }
}
