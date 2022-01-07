using Data.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IInformationService
    {
        Task<List<Container>> GetVehicleRoute(int id);

        List<ContainerCluster> CreateClusters(int clusterCount);
    }
}
