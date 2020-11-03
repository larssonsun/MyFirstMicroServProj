using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.Consign.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConsignController : ControllerBase
    {
        private readonly ILogger<ConsignController> _logger;
        private readonly ICapPublisher _capBus;
        private readonly IConsignRepository _consignRepository;

        public ConsignController(ILogger<ConsignController> logger, ICapPublisher capBus, IConsignRepository consignRepository)
        {
            _logger = logger;
            _capBus = capBus;
            _consignRepository = consignRepository;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateConsign()
        {
            var result = await _consignRepository.CreateConsign(new Consign() { Id = Guid.NewGuid() });

            if (!result)
            {
                return StatusCode(500, "委托创建失败");
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsignDTO>>> GetConsign([FromQuery] QueryFilterDTO queryFilter)
        {
            var claims = Request.Headers["IsFukinUser"];
            Console.WriteLine("----------" + claims[0]);

            var consign = await _consignRepository.GetConsigns(queryFilter);

            return Ok(consign.Select(x => new ConsignDTO()
            {
                ConsignNo = x.ConsignNo,
                Memo = "larsson",
                DTOCreateTime = DateTime.Now.ToString()
            }));
        }
    }
}
