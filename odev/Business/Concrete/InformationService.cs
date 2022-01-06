using Business.Abstract;
using Core.Abstract;
using Core.Clustering;
using Data.Domain.Entities;
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

        public InformationService(
            IRepository<Container> containerRepository,
            IRepository<Vehicle> vehicleRepository
            )
        {
            _containerRepository = containerRepository;
            _vehicleRepository = vehicleRepository;
        }

        public List<ContainerCluster> CreateClusters()
        {
            var containers =   _containerRepository.GetAllAsync().Result;
            var clusterList = new List<ContainerCluster>();
            foreach(var con in containers)
            {
                clusterList.Add(new ContainerCluster { ContainerId = con.Id, LocationX = con.LocationX , LocationY = con.LocationY});
            }
            var clusterCount = clusterList.Count()/ 8 +1;

            var kmeans = new KMeans();
            var res =  kmeans.Cluster(clusterList, clusterCount);
            return res;
        }

        public async Task<List<Container>> GetVehicleRoute(int id)
        {
            var containers = await _containerRepository.GetAllAsync();
            return containers.Where(x => x.VehicleId == id).ToList();
        }
    }
}
