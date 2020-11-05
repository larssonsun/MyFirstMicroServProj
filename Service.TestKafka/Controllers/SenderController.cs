using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.TestKafka
{
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private readonly ILogger<SenderController> _logger;
        private readonly ICapPublisher _capBus;

        public SenderController(ILogger<SenderController> logger, ICapPublisher capBus)
        {
            _logger = logger;
            _capBus = capBus;
        }

        [HttpGet]
        public IActionResult SendMessageToKafka()
        {
            _capBus.Publish("test.show.time", DateTime.Now);
            Console.WriteLine("sender send the message to kafka.");
            return Ok();
        }
    }
}
