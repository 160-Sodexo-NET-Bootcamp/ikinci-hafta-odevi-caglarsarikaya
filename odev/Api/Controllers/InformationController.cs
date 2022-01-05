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

        [HttpGet]
        public Task<List<Container>> GetVehicleRoute(int Id)
        {
           return  _informationService.GetVehicleRoute(Id);
        }


    }
}
