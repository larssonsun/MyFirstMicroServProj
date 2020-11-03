using System;
using System.Threading.Tasks;
using RestSharp;

namespace Client.Con
{
    public class ScetiFromOcelotService : IScetiService
    {
        private readonly IConfiguration _configuration;

        public ScetiFromOcelotService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateConsign()
        {
            var Client = new RestClient(_configuration.GetVal("OcelotSetting:OcelotAddress"));
            var request = new RestRequest("/sceti/consign", Method.POST);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetConsigns()
        {
            var Client = new RestClient(_configuration.GetVal("OcelotSetting:OcelotAddress"));
            var request = new RestRequest("/sceti/consign", Method.GET)
                .AddQueryParameter("pageSize", "3")
                .AddQueryParameter("pageNo", "2");

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            throw new NotImplementedException();
        }
    }
}