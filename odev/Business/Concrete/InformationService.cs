using Business.Abstract;
using Core.Abstract;
using Core.Clustering;
using Data.Domain.Entities;
using Data.IUnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class InformationService : IInformationService
    {
        private readonly IRepository<Container> _containerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<ContainerCluster> _containerClusterRepository;
        private readonly IUnitOfWork _uow;

        public InformationService(
            IRepository<Container> containerRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<ContainerCluster> containerClusterRepository,
             IUnitOfWork uow
            )
        {
            _containerRepository = containerRepository;
            _vehicleRepository = vehicleRepository;
            _containerClusterRepository = containerClusterRepository;
            _uow = uow;
        }

        public List<ContainerCluster> CreateClusters()
        {
            var containers = _containerRepository.GetAllAsync().Result;
            var clusterList = new List<ContainerCluster>();
            foreach (var con in containers)
            {
                clusterList.Add(new ContainerCluster { ContainerId = con.Id, LocationX = con.LocationX, LocationY = con.LocationY });
            }
            var clusterCount = clusterList.Count() / 8 + 1;

            var kmeans = new KMeans();
            var res = kmeans.Cluster(clusterList, clusterCount);



            var savedClusters = _containerClusterRepository.GetAllAsync().Result;
            _uow.TrackerClear();
             foreach(var willRecord in res)
            {
                var updateable = savedClusters.Where(x => x.ContainerId == willRecord.ContainerId).SingleOrDefault();
                if(updateable != null)
                {
                    willRecord.Id = updateable.Id;  
                    _containerClusterRepository.Update(willRecord);
                }
                else
                {
                    _containerClusterRepository.AddAsync(willRecord);

                }
            }
            _uow.SaveChanges();


            return res;
        }

        public async Task<List<Container>> GetVehicleRoute(int id)
        {
            var containers = await _containerRepository.GetAllAsync();
            return containers.Where(x => x.VehicleId == id).ToList();
        }
    }
}
