using System;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Service.TestKafka
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiverController : ControllerBase
    {
        private readonly ILogger<ReceiverController> _logger;
        private readonly ICapPublisher _capBus;

        public ReceiverController(ILogger<ReceiverController> logger, ICapPublisher capBus)
        {
            _logger = logger;
            _capBus = capBus;
        }

        [NonAction]
        [CapSubscribe("test.show.time")]
        public IActionResult ReveiveMessageFromKafka(DateTime time)
        {
            Console.WriteLine("reveived the message from kafka: " + time.ToString("yyyy-MM-dd HH:mm:ss"));
            return Ok();
        }
    }
}
