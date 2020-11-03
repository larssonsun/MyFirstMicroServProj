using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;
        private readonly ICapPublisher _capBus;
        private readonly ISampleRepository _sampleRepositry;

        public SampleController(ILogger<SampleController> logger, ICapPublisher capBus, ISampleRepository sampleRepositry)
        {
            _logger = logger;
            _capBus = capBus;
            _sampleRepositry = sampleRepositry;
        }
    }
}
