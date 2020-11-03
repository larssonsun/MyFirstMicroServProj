using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using RestSharp;

namespace Client.Con
{
    public class ScetiService : IScetiService
    {
        private readonly IConfiguration _configuration;
        private readonly ConsulClient _consulClient;
        private ConcurrentDictionary<string, string[]> _serviceUrls = new ConcurrentDictionary<string, string[]>();

        public ScetiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consulClient = new ConsulClient(c =>
            {
                //consul地址
                c.Address = new Uri(_configuration.GetVal("ConsulSetting:ConsulAddress"));
            });
        }

        public async Task<string> CreateConsign()
        {
            var url = _serviceUrls.SingleOrDefault(x => x.Key == "ConsignService").Value;
            if (url == null)
                return await Task.FromResult("【委托服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(url.ElementAt(new Random().Next(0, url.Count())));
            var request = new RestRequest("/consign", Method.POST);

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public async Task<string> GetConsigns()
        {
            var url = _serviceUrls.SingleOrDefault(x => x.Key == "ConsignService").Value;
            if (url == null)
                return await Task.FromResult("【委托服务】正在初始化服务列表...");

            //每次随机访问一个服务实例
            var Client = new RestClient(url.ElementAt(new Random().Next(0, url.Count())));
            var request = new RestRequest("/consign", Method.GET)
                .AddQueryParameter("pageSize", "3")
                .AddQueryParameter("pageNo", "2");

            var response = await Client.ExecuteAsync(request);
            return response.Content;
        }

        public void GetServices()
        {
            var serviceNames = new string[] { "ConsignService" };
            Array.ForEach(serviceNames, async p =>
            {
                //WaitTime默认为5分钟
                var queryOptions = new QueryOptions { WaitTime = TimeSpan.FromSeconds(20) };
                while (true)
                {
                    await GetServicesAsync(queryOptions, p);
                }
            });
        }

        private async Task GetServicesAsync(QueryOptions queryOptions, string serviceName)
        {
            var res = await _consulClient.Health.Service(serviceName, null, true, queryOptions);

            //控制台打印一下获取服务列表的响应时间等信息
            Console.WriteLine($"{DateTime.Now}获取{serviceName}：queryOptions.WaitIndex：{queryOptions.WaitIndex}  LastIndex：{res.LastIndex}");

            //版本号不一致 说明服务列表发生了变化
            if (queryOptions.WaitIndex != res.LastIndex)
            {
                Console.WriteLine("版本号不一致，重新从consul获取服务列表");
                queryOptions.WaitIndex = res.LastIndex;

                //服务地址列表
                var serviceUrlsFromConsul = res.Response.Select(p => $"http://{p.Service.Address + ":" + p.Service.Port}").ToArray();
                _serviceUrls.TryAdd(serviceName, serviceUrlsFromConsul);
            }
        }
    }
}