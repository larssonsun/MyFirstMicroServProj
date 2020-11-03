using System;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace Service.Sample
{
    public interface ISampleRepository
    {
        Task<bool> CreateSample(ConsignCreatedResult consignCreatedResult);
    }

    public class SampleRepository : ICapSubscribe, ISampleRepository
    {
        private readonly SampleContext _sampleContext;

        public SampleRepository(SampleContext sampleContext)
        {
            _sampleContext = sampleContext;
        }

        [CapSubscribe("consign.main")]
        public async Task<bool> CreateSample(ConsignCreatedResult consignCreatedResult)
        {
            Console.WriteLine("consignNo:" + consignCreatedResult.ConsignNo);
            _sampleContext.Add(new Sample
            {
                Id = Guid.NewGuid(),
                ConsignNo = consignCreatedResult.ConsignNo,
                SampleNo = SampleNoGernerator(consignCreatedResult.ConsignNo)
            });

            await _sampleContext.SaveChangesAsync();

            return true;
        }

        private string SampleNoGernerator(string consignNo)
        {
            return consignNo + "-1";
        }
    }
}