using Api.DTO;
using AutoMapper;
using Business.Abstract;
using Data.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;
        private readonly IMapper _mapper;
        public ContainerController(IContainerService containerService, IMapper mapper)
        {
            _containerService = containerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<Container>> GetAll()
        {
            return await _containerService.GetAll();
        }

        [HttpDelete]
        public void Delete(int id)
        { 
            _containerService.Delete(id);
        }
        [HttpPost]
        public async Task Insert(ContainerDTO container)
        {
            var mapped = _mapper.Map<Container>(container);
            await _containerService.Insert(mapped);
        }

        [HttpPut]
        public void Update(Container container)
        {
            _containerService.Update(container);
        }
    }
}
