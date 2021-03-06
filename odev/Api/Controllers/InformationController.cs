using Business.Abstract;
using Data.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly IInformationService _informationService;

        public InformationController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        [HttpGet("GetVehicleRoute")]
        public Task<List<Container>> GetVehicleRoute(int Id)
        {
            return _informationService.GetVehicleRoute(Id);
        }

        [HttpGet("CreateClusters/{clusterCount}")]
        public List<ContainerCluster> CreateClusters(int clusterCount)
        {
            return _informationService.CreateClusters(clusterCount);
        }


    }
}
